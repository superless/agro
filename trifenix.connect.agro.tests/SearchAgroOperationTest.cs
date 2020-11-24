using AutoMapper;
using Microsoft.Spatial;
using Newtonsoft.Json;
using System.Linq;
using System.Text.RegularExpressions;
using trifenix.agro.external.operations.tests.data;
using trifenix.connect.agro.external;
using trifenix.connect.agro.external.hash;
using trifenix.connect.agro.external.helper;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.queries;
using trifenix.connect.agro.tests.mock;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.mdm.entity_model;
using trifenix.connect.mdm.ts_model;
using trifenix.connect.tests.mock;
using Xunit;

namespace trifenix.connect.agro.tests
{
    public class SearchAgroOperationTest
    {



        public SearchAgroOperationTest()
        {

        }
        

        




        /// <summary>
        /// Obtiene un producto entitySearch desde un elemento de la base de datos (DocumentBase)
        /// </summary>
        [Fact]
        public void GetEntitySearchFromProduct()
        {
            // assign

            var agroSearchOperation = new AgroSearch<GeoPointTs>(MockHelper.BaseSearch(), new SearchQueries(), new ImplementMock(), new HashEntityAgroSearch());


            var prd = AgroInputData.Product1;


            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ProductInput, Product>());

            var mapperLocal = mapper.CreateMapper();
            var prdModel = mapperLocal.Map<Product>(prd);

            // action
            var result = agroSearchOperation.GetEntitySearch(prdModel);
            // assert
            Assert.True(result.First().index == 12);
        }


        /// <summary>
        /// Verifica que las ids creadas de manera automática en waiting harvest, se hayan asignado a cada waiting harvest 
        /// y que existan en las elementos de relaciones de un entitySearch de tipo dosis.
        /// </summary>
        [Fact]
        public void GetEntitySearchFromDosesCheckCircleId()
        {
            // assign

            var agroSearchOperation = new AgroSearch<GeoPointTs>(MockHelper.BaseSearch(), new SearchQueries(), new ImplementMock(), new HashEntityAgroSearch());


            var prd = AgroInputData.Product1;

            prd.Doses[0].Id = ConstantGuids.Value[0];

            var doseInput = prd.Doses[0];
            var doseModel = new Dose
            {
                Active = doseInput.Active,
                ApplicationDaysInterval = doseInput.ApplicationDaysInterval,
                Default = doseInput.Default,                
                DosesQuantityMax = doseInput.DosesQuantityMax,
                IdSpecies = doseInput.IdSpecies,
                DosesQuantityMin = doseInput.DosesQuantityMin,
                IdProduct = doseInput.IdProduct,
                NumberOfSequentialApplication = doseInput.NumberOfSequentialApplication,
                HoursToReEntryToBarrack = doseInput.HoursToReEntryToBarrack,
                IdVarieties = doseInput.IdVarieties,
                IdsApplicationTarget = doseInput.IdsApplicationTarget,
                WaitingDaysLabel = doseInput.WaitingDaysLabel,
                WaitingToHarvest = doseInput.WaitingToHarvest.Select(s => new WaitingHarvest { IdCertifiedEntity = s.IdCertifiedEntity, Ppm = s.Ppm, WaitingDays = s.WaitingDays }).ToList(),
                Id = doseInput.Id,
                WettingRecommendedByHectares = doseInput.WettingRecommendedByHectares
            };

            // action
            var result = agroSearchOperation.GetEntitySearch(doseModel);

            var waitingHarvesRelated = result.First(s => s.index == (int)EntityRelated.DOSES).rel.Where(s => s.index == (int)EntityRelated.WAITINGHARVEST).ToList();

            var waitingHarvestEntitySearchs = result.Where(q => q.index == (int)EntityRelated.WAITINGHARVEST).Select(s => s.id).ToList();
            // assert
            Assert.True(waitingHarvesRelated.All(a => waitingHarvestEntitySearchs.Any(o => o.Equals(a.id))));
        }

        /// <summary>
        /// Añade un producto al search, confirma los hash de cabecera y modelo.
        /// </summary>
        [Fact]
        public void AddProductToSearch()
        {
            // assign
            // hash operaciones para asignar hash de cabecera y modelo
            var hash = new HashEntityAgroSearch();
            var agroSearchOperation = new AgroSearch<GeoPointTs>(MockHelper.BaseSearch(), new SearchQueries(), new ImplementMock(), hash);


            // producto input
            var prd = AgroInputData.Product1;


            // Convierte en objeto
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ProductInput, Product>());
            var mapperLocal = mapper.CreateMapper();
            var prdModel = mapperLocal.Map<Product>(prd);



            // action
            agroSearchOperation.AddDocument(prdModel);

            // obtiene los elementos a guardar (entitySearchs)
            var jsonElement = JsonConvert.DeserializeObject<EntityMockSearch[]>(agroSearchOperation.Queried.First().Value.First());

            // hash de cabeceras de producto
            var hh = hash.HashHeader(prdModel.GetType());

            // hash del producto
            var hm = hash.HashModel(prdModel);


            // assert
            // Confirma que los hash confirmen el valor.
            Assert.True(jsonElement.FirstOrDefault().hm.Equals(hm) && jsonElement.FirstOrDefault().hh.Equals(hh));

        }

        /// <summary>
        /// Verifica que al obtener un elemento desde azure search,
        /// la consulta coincida.
        /// </summary>
        [Fact]
        public void VerificandoConsultaAzureSearchGetEntity()
        {
            // assign
            var agroSearchOperation = new AgroSearch<GeoPointTs>(MockHelper.BaseSearch(), new SearchQueries(), new ImplementMock(), new HashEntityAgroSearch());
            //action
            agroSearchOperation.GetEntity(EntityRelated.BARRACK, ConstantGuids.Value[0]);


            var queries = agroSearchOperation.Queried;

            //assert
            Assert.Contains(queries["GetEntity"], s => s.Equals("index eq 1 and id eq '9aebaf15-eb85-49d7-acca-643329d4078b'"));
            

        }


        /// <summary>
        /// Verifica que la consulta utilizada para borrar un entitySearch coincida correctamente con la operación.
        /// </summary>
        [Fact]
        public void VerificandoConsultaAzureSearchDeleteEntity()
        {
            // assign
            var agroSearchOperation = new AgroSearch<GeoPointTs>(MockHelper.BaseSearch(), new SearchQueries(), new ImplementMock(), new HashEntityAgroSearch());
            //action
            agroSearchOperation.DeleteEntity(EntityRelated.BARRACK, ConstantGuids.Value[0]);


            var queries = agroSearchOperation.Queried;

            Assert.Contains(queries["DeleteEntity"], s => s.Equals("index eq 1 and id eq '9aebaf15-eb85-49d7-acca-643329d4078b'"));
            //assert

        }


        /// <summary>
        /// Elimina todas las dosis de un producto identificado, se verifica consulta de eliminación.
        /// </summary>
        [Fact]
        public void VerificandoConsultaAzureSearchDeleteEntityWithRelatedElement()
        {
            // assign
            var agroSearchOperation = new AgroSearch<GeoPointTs>(MockHelper.BaseSearch(), new SearchQueries(), new ImplementMock(), new HashEntityAgroSearch());
            //action
            agroSearchOperation.DeleteElementsWithRelatedElement(EntityRelated.DOSES, EntityRelated.PRODUCT, ConstantGuids.Value[1]);


            var queries = agroSearchOperation.Queried;

            // borra todos las dosis de un producto con id = '7990893f-74e1-45d6-8f3d-af1c9896842c'
            Assert.Contains(queries["DeleteElementsWithRelatedElement"], s => s.Equals("index eq 6  and rel/any(elementId: elementId/index eq 12 and elementId/id eq '7990893f-74e1-45d6-8f3d-af1c9896842c')"));
            //assert

        }

        /// <summary>
        /// Obtiene las dosis desde el search que tengan un id de producto asociado.
        /// </summary>
        [Fact]
        public void VerificandoConsultaAzureSearchGetElementsWithRelatedElement()
        {
            // assign
            var agroSearchOperation = new AgroSearch<GeoPointTs>(MockHelper.BaseSearch(), new SearchQueries(), new ImplementMock(), new HashEntityAgroSearch());
            //action
            agroSearchOperation.GetElementsWithRelatedElement(EntityRelated.DOSES, EntityRelated.PRODUCT, ConstantGuids.Value[1]);


            var queries = agroSearchOperation.Queried;

            // es la misma consulta usada para borrar
            Assert.Contains(queries["GetElementsWithRelatedElement"], s => s.Equals("index eq 6  and rel/any(elementId: elementId/index eq 12 and elementId/id eq '7990893f-74e1-45d6-8f3d-af1c9896842c')"));
            //assert

        }


        /// <summary>
        /// Elimina todas las dosis de un producto excepto producto con id.
        /// se verifica consulta a azure search.
        /// </summary>
        [Fact]
        public void VerificandoConsultaAzureSearchDeleteElementsWithRelatedElementExceptId()
        {
            // assign
            var agroSearchOperation = new AgroSearch<GeoPointTs>(MockHelper.BaseSearch(), new SearchQueries(), new ImplementMock(), new HashEntityAgroSearch());
            //action
            agroSearchOperation.DeleteElementsWithRelatedElementExceptId(EntityRelated.DOSES, EntityRelated.PRODUCT, ConstantGuids.Value[1], ConstantGuids.Value[2]);


            var queries = agroSearchOperation.Queried;

            // Elimina todas las dosis excepto la que contiene el id 2, y el producto con el id 1.
            Assert.Contains(queries["DeleteElementsWithRelatedElementExceptId"], s => s.Equals($"index eq 6 and  id ne '{ConstantGuids.Value[2]}' and rel/any(elementId: elementId/index eq 12 and elementId/id eq '{ConstantGuids.Value[1]}')"));
            //assert

        }





    }


}
