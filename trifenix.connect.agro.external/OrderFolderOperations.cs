using System;
using System.Threading.Tasks;
using trifenix.connect.agro.external.main;
using trifenix.connect.agro.interfaces;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.interfaces.db.cosmos;
using trifenix.connect.interfaces.external;
using trifenix.connect.mdm.containers;

namespace trifenix.connect.agro.external
{
    /// <summary>
    /// Operaciones de las carpetas de ordenes,
    /// se encarga del almacenamiento de los parametros ingresados
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OrderFolderOperations<T> : MainOperation<OrderFolder, OrderFolderInput,T>, IGenericOperation<OrderFolder, OrderFolderInput> {
        

        public OrderFolderOperations(IMainGenericDb<OrderFolder> repo, IAgroSearch<T> search, ICommonAgroQueries commonQueries, ICommonDbOperations<OrderFolder> commonDb, IValidatorAttributes<OrderFolderInput> validator) : base(repo, search, commonDb, validator) {
            
        }
 

        public override async Task<ExtPostContainer<string>> SaveInput(OrderFolderInput input) {
            await Validate(input);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var orderFolder = new OrderFolder {
                Id = id,
                IdApplicationTarget = input.IdApplicationTarget,
                IdIngredientCategory = input.IdIngredientCategory,
                IdIngredient = input.IdIngredient,
                IdPhenologicalEvent = input.IdPhenologicalEvent,
                IdSpecie = input.IdSpecie
            };
             await SaveDb(orderFolder);
            return await SaveSearch(orderFolder);
        }


        
    }

}