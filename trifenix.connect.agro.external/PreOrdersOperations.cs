using System;
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
    
        public async override Task Validate(PreOrderInput input)
        {
            /// <summary>
            /// Solo una pre orden puede tener el mismo ingrediente activo que el Order Folder indexado, por lo que se busca 
            /// el ingrediente activo de esta y se compara con el ingrediente que se está ingresando
            /// </summary>
            var OFIngredient = await Queries.GetOrderFolderIngredientFromPreOrder(input.OrderFolderId);
            
            if (input.IngredientId == OFIngredient)
            {
                /// <summary>
                /// Se obtienen todos los ingredientes que tengan la pre orden asociada para ver si ya existe una pre orden con
                /// el ingrediente de la order folder
                /// </summary>
                var POIngredients = await Queries.GetPreOrderIngredientFromOrderFolder(input.OrderFolderId);
                foreach (var item in POIngredients)
                {
                    if (input.IngredientId == item)
                    {
                        throw new CustomException("El ingrediente de la carpeta de ordenes ya se encuentra en uso");
                    }
                }
            }
            /// <summary>
            /// Barracks deben ser unicos al momento de ingresar
            /// </summary>
            bool isUnique = input.BarrackIds.Distinct().Count() == input.BarrackIds.Count();
            if (!isUnique)
            {
                throw new CustomException("No se pueden ingresar barracks duplicados");
            }

            if (!input.BarrackIds.Any())
            {
                throw new CustomException("No se puede ingresar una pre orden sin un barrack asociado");
            }

            if (!Enum.IsDefined(typeof(PreOrderType), input.PreOrderType))
                throw new ArgumentOutOfRangeException("input","Enum fuera de rango");

            await base.Validate(input);
        }

        public override async Task<ExtPostContainer<string>> SaveInput(PreOrderInput input)
        {
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");

            /// Valida cada pre orden
            await Validate(input);

            var preOrder = new PreOrder
            {
                Id = id,
                Name = input.Name,
                OrderFolderId = input.OrderFolderId,
                PreOrderType = input.PreOrderType,
                Ingredient = input.IngredientId,
                BarrackIds = input.BarrackIds
            };

            await SaveDb(preOrder);
            var result = await SaveSearch(preOrder);
            return result;
        }   
 
    }

}