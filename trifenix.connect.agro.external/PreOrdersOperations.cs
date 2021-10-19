using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.connect.agro.external.main;
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.interfaces.db;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.interfaces.db;
using trifenix.connect.interfaces.external;
using trifenix.connect.mdm.containers;
using trifenix.exception;

namespace trifenix.connect.agro.external
{
    public class PreOrdersOperations<T> : MainOperation<PreOrder, PreOrderInput, T>, IGenericOperation<PreOrder, PreOrderInput>
    {
        private readonly ICommonAgroQueries Queries;

        public PreOrdersOperations(IMainGenericDb<PreOrder> repo, IAgroSearch<T> search, ICommonAgroQueries queries, IValidatorAttributes<PreOrderInput> validator, ILogger log) : base(repo, search, validator, log)
        {
            Queries = queries;
        }

        private async Task<bool> IsRepeated(string BarrackId, PreOrderInput input)
        {
            var OFAtt = await Queries.GetOFAttributes(input.OrderFolderId);
            var rs = OFAtt.FirstOrDefault();
            var idPE = rs["IdPhenologicalEvent"];
            var idAT = rs["IdApplicationTarget"];
            var idSP = rs["IdSpecie"];

            // obtiene los order folder que coinciden con los filtros
            var similarOF = await Queries.GetSimilarOF(idPE, idAT, idSP);

            foreach (var item in similarOF)
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

            var OFBarracks = await Queries.GetOFBarracks(input.Id);
            var OFBarracksGroup = OFBarracks.SelectMany(s => s).ToList();
            // identificador de la especie de un order folder.
            var OFSpecie = await Queries.GetOFSpecie(input.OrderFolderId);

            List<string> newBarracks = new List<string>();
            foreach (var item in input.BarrackIds)
            {
                if (!OFBarracksGroup.Contains(item))
                {
                    newBarracks.Add(item);
                }
            }
            // recorre los ids de los barracks de un input.
            foreach (var item in newBarracks)
            {
                var variety = await Queries.GetBarrackVarietyFromBarrackId(item);
                var specie = await Queries.GetSpecieFromVarietyId(variety);
                if (specie != OFSpecie)
                {
                    throw new CustomException($"La especie del barrack de id {item} no es la misma que la especie de la order folder a la que quiere ser ingresado");
                }
                await IsRepeated(item, input);
            }

            if (!Enum.IsDefined(typeof(PreOrderType), input.PreOrderType))
                throw new ArgumentOutOfRangeException("input", "Enum fuera de rango");

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