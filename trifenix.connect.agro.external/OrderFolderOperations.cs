using System;
using System.Threading.Tasks;
using trifenix.connect.agro.external.main;
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
    /// <summary>
    /// Operaciones de las carpetas de ordenes,
    /// se encarga del almacenamiento de los parametros ingresados
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OrderFolderOperations<T> : MainOperation<OrderFolder, OrderFolderInput,T>, IGenericOperation<OrderFolder, OrderFolderInput> {
        private readonly ICommonAgroQueries Queries;

        public OrderFolderOperations(IMainGenericDb<OrderFolder> repo, IAgroSearch<T> search, ICommonAgroQueries Queries, IValidatorAttributes<OrderFolderInput> validator) : base(repo, search, validator) {
            this.Queries = Queries;
        }
 
        public async override Task Validate(OrderFolderInput input)
        {
            /// <summary>
            /// Comprueba si existen order folders duplicados
            /// </summary>
            var orders = await Queries.GetDuplicatedOrderFolders(input.IdApplicationTarget, input.IdIngredient, input.IdPhenologicalEvent, input.IdSpecie);
            var NumOrders = int.Parse(orders);
            if(NumOrders != 0)
            {
                throw new CustomException("No se pueden ingresar ordenes repetidas");
            }
        }

        public override async Task<ExtPostContainer<string>> SaveInput(OrderFolderInput input) {
            await Validate(input);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var orderFolder = new OrderFolder {
                Id = id,
                IdApplicationTarget = input.IdApplicationTarget,
                IdIngredient = input.IdIngredient,
                IdPhenologicalEvent = input.IdPhenologicalEvent,
                IdSpecie = input.IdSpecie
            };
             await SaveDb(orderFolder);
            return await SaveSearch(orderFolder);
        }


        
    }

}