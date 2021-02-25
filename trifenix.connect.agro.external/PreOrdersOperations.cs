using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.connect.agro.external.main;
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.interfaces;
using trifenix.connect.agro.interfaces.cosmos;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.interfaces.db.cosmos;
using trifenix.connect.interfaces.external;
using trifenix.connect.mdm.containers;
using trifenix.exception;

namespace trifenix.connect.agro.external
{
    public class PreOrdersOperations<T> : MainOperation<PreOrder, PreOrderInput,T>, IGenericOperation<PreOrder, PreOrderInput> {
        private readonly ICommonAgroQueries Queries;

        public PreOrdersOperations(IDbExistsElements existsElement, IMainGenericDb<PreOrder> repo, IAgroSearch<T> search, ICommonDbOperations<PreOrder> commonDb, ICommonAgroQueries queries, IValidatorAttributes<PreOrderInput> validator) : base(repo, search, commonDb, validator) { 
            Queries = queries;
        }

        private async Task<bool> IsRepeated(string BarrackId, PreOrderInput input)
        {
            // obtiene los parámetros de filtro del order folder.
            var OFAtt = await Queries.GetOFAttributes(input.OrderFolderId);
            
            var rs = OFAtt.FirstOrDefault();
            var idPE = rs["IdPhenologicalEvent"];
            var idAT = rs["IdApplicationTarget"];
            var idSP = rs["IdSpecie"];


            // obtiene los order folder que coinciden con los filtros
            var similarOF = await Queries.GetSimilarOF(idPE, idAT, idSP);



            foreach(var item in similarOF)
            {
                var aB = await Queries.GetBarracksFromOrderFolderId(item);
                var manyBarracks = aB.SelectMany(s => s).ToList();
                var isRepeated = manyBarracks.Contains(BarrackId);
                if (isRepeated)
                {
                    throw new CustomException("El barrack ya existe en la order folder");
                }
            }
            return false;
        }

        public async override Task Validate(PreOrderInput input)
        {
            // si el id no tiene valor es un nuevo valor
            // si el id tiene valor es edición

            await base.Validate(input);

            if (!input.BarrackIds.Any())
            {
                throw new CustomException("No se puede ingresar una pre orden sin un barrack asociado");
            }

            bool isUnique = input.BarrackIds.Distinct().Count() == input.BarrackIds.Count();
            if (!isUnique)
            {
                throw new CustomException("No se pueden ingresar barracks duplicados");
            }
            

            // identificador de la especie de un order folder.
            var OFSpecie = await Queries.GetOFSpecie(input.OrderFolderId);


            // recorre los ids de los barracks de un input.
            for (int i = 0; i < input.BarrackIds.Length; i++)
            {
                // obtienr el id de la variedad del barrack.
                var variety = await Queries.GetBarrackVarietyFromBarrackId(input.BarrackIds[i]);

                // obtiene la especie
                var specie = await Queries.GetSpecieFromVarietyId(variety);

                // si la especie no es igual a la de la orderfolder, lanza error. 
                if (specie != OFSpecie)
                {
                    throw new CustomException($"La especie del barrack de id {input.BarrackIds[i]} no es la misma que la especie de la order folder a la que quiere ser ingresado");
                }

                // valida si está repetido.
                await IsRepeated(input.BarrackIds[i], input);
            }




            if (!Enum.IsDefined(typeof(PreOrderType), input.PreOrderType))
                throw new ArgumentOutOfRangeException("input","Enum fuera de rango");

            
        }

        public override async Task<ExtPostContainer<string>> SaveInput(PreOrderInput input)
        {
            /// Valida cada pre orden
            await Validate(input);

            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");

            

            var preOrder = new PreOrder
            {
                Id = id,
                Name = input.Name,
                OrderFolderId = input.OrderFolderId,
                PreOrderType = input.PreOrderType,
                BarrackIds = input.BarrackIds
            };

            await SaveDb(preOrder);
            var result = await SaveSearch(preOrder);
            return result;
        }   
 
    }

}