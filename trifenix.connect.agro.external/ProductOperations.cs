using System;
using System.Linq;
using System.Threading.Tasks;
using trifenix.connect.agro.external.main;
using trifenix.connect.agro.interfaces;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.interfaces.db.cosmos;
using trifenix.connect.interfaces.external;
using trifenix.connect.mdm.containers;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro.external
{


    /// <summary>
    /// Operación de productos
    /// </summary>
    /// <typeparam name="T">Tipo Geo para base de datos de busqueda</typeparam>
    public class ProductOperations<T> : MainOperation<Product, ProductInput, T>, IGenericOperation<Product, ProductInput> {

        /// <summary>
        /// Operaciones de base de datos para dosis.
        /// </summary>
        private readonly IGenericOperation<Dose, DosesInput> dosesOperation;

        /// <summary>
        /// Consultas a base de datos
        /// </summary>
        private readonly ICommonAgroQueries queries;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="repo">repositorio de base de datos de productos</param>
        /// <param name="search">operaciones en base de datos de busqueda basadas en entitySearch</param>
        /// <param name="dosesOperation">Operaciones de dosis</param>
        /// <param name="commonDb">convierte IQueryable en listas</param>
        /// <param name="queries">consultas a agro</param>
        /// <param name="validator">Validador de elementos</param>
        public ProductOperations(IMainGenericDb<Product> repo, IAgroSearch<T> search, IGenericOperation<Dose, DosesInput> dosesOperation, ICommonDbOperations<Product> commonDb, ICommonAgroQueries queries, IValidatorAttributes<ProductInput> validator) : base(repo, search, commonDb, validator) {
            this.dosesOperation = dosesOperation;
            this.queries = queries;
        }
        
        /// <summary>
        /// Elimina una entidad producto, se debe considerar la eliminación inicial de las dosis.
        /// </summary>
        /// <param name="id">identificador del elemento a eliminar</param>
        /// <returns></returns>
        public override async Task Remove(string id) {
            
            await repo.DeleteEntity(id);
        }


        
        /// <summary>
        /// Crea una dosis por defecto, 
        /// todo producto debe tener una dosis por defecto.
        /// </summary>
        /// <param name="idProduct">identificador del producto</param>
        /// <returns></returns>
        private async Task<string> CreateDefaultDoses(string idProduct) {
            var dosesInput = new DosesInput {
                IdProduct = idProduct,
                Active = true,
                Default = true
            };


            // guarda en base de datos de operación.
            var result = await dosesOperation.SaveInput(dosesInput);


            return result.IdRelated;
        }

        
        /// <summary>
        /// Remueve las dosis de producto
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        private async Task RemoveDoses(Product product) {
            if (!string.IsNullOrWhiteSpace(product.Id)) {
                //obtiene el identificador de la dosis por defecto
                var defaultDoses = await queries.GetDefaultDosesId(product.Id);

                if (string.IsNullOrWhiteSpace(defaultDoses))
                    await CreateDefaultDoses(product.Id);


                // obtiene todas las dosis que no sean por defecto y estén activas
                var dosesPrevIds = await queries.GetActiveDosesIdsFromProductId(product.Id);


                // de las dosis encontradas usa el dosesOperation para eliiminar
                if (dosesPrevIds.Any())
                    foreach (var idDoses in dosesPrevIds) {
                        // elimina cada dosis, internamente elimina si no hay dependencias, si existen dependencias la desactiva y la deja en el search.
                        await dosesOperation.Remove(idDoses);
                    }
                
            }
        }


        
        /// <summary>
        /// Guarda un producto en una base de dato de busqueda y luego una de persistencia.
        /// </summary>
        /// <param name="productInput">input de usuario</param>
        /// <returns>Exito si logró realizar correctamente el guardado</returns>
        public override async Task<ExtPostContainer<string>> SaveInput(ProductInput productInput) {
            
            await Validate(productInput);

            var id = !string.IsNullOrWhiteSpace(productInput.Id) ? productInput.Id : Guid.NewGuid().ToString("N");
            if (productInput.Doses.Any(s=>!string.IsNullOrWhiteSpace(s.Id)))
            {
                return new ExtPostContainer<string>()
                {
                    IdRelated = id,
                    MessageResult = ExtMessageResult.BadInput,
                    Message = "Al guardar un producto se eliminan su dosis y se vuelven a crear, las dosis no deben llevar identificación"
                };
            }

            var product = new Product {
                Id = id,
                IdBrand = productInput.IdBrand,
                Name = productInput.Name,
                IdActiveIngredient = productInput.IdActiveIngredient,
                SagCode = productInput.SagCode,
                MeasureType = productInput.MeasureType,
                
            };

            // 1. guardar producto en base de datos.
            var postResult = await SaveDb(product);

            // 2. remueve dosis según corresponda
            await RemoveDoses(product);

            // 3. asigna los id de producto a las dosis.
            var doses = productInput.Doses.Select(dose => {
                dose.IdProduct = id;
                return dose;
            });

            //4. guarda cada operación
            foreach (var dose in doses) {
                var saveDoses =  await dosesOperation.SaveInput(dose);
                if (saveDoses.MessageResult != ExtMessageResult.Ok)
                {
                    return saveDoses;
                }
            }

            // guarda en producto
            return await SaveSearch(product);

        }

        

        
    }

}