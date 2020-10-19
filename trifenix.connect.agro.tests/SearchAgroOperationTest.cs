using System.Linq;
using trifenix.agro.external.operations.tests.data;
using trifenix.connect.agro.external.helper;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.queries;
using trifenix.connect.agro.tests.mock;
using trifenix.connect.mdm.ts_model;
using Xunit;

namespace trifenix.connect.agro.tests
{
    public class SearchAgroOperationTest
    {



        public SearchAgroOperationTest()
        {

        }


        /// <summary>
        /// Verifica que al obtener un elemento desde azure search,
        /// la consulta coincida.
        /// </summary>
        [Fact]
        public void VerificandoConsultaAzureSearchGetEntity()
        {
            // assign
            var agroSearchOperation = new SearchQueryOperations<GeoPointTs>(MockHelper.BaseSearch(), new SearchQueries());
            //action
            agroSearchOperation.GetEntity(EntityRelated.BARRACK, ConstantGuids.Value[0]);


            var queries = agroSearchOperation.Queried;

            Assert.Contains(queries["GetEntity"], s => s.Equals("index eq 1 and id eq '9aebaf15-eb85-49d7-acca-643329d4078b'"));
            //assert

        }


        /// <summary>
        /// Verifica que la consulta utilizada para borrar un entitySearch coincida correctamente con la operación.
        /// </summary>
        [Fact]
        public void VerificandoConsultaAzureSearchDeleteEntity()
        {
            // assign
            var agroSearchOperation = new SearchQueryOperations<GeoPointTs>(MockHelper.BaseSearch(), new SearchQueries());
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
            var agroSearchOperation = new SearchQueryOperations<GeoPointTs>(MockHelper.BaseSearch(), new SearchQueries());
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
            var agroSearchOperation = new SearchQueryOperations<GeoPointTs>(MockHelper.BaseSearch(), new SearchQueries());
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
            var agroSearchOperation = new SearchQueryOperations<GeoPointTs>(MockHelper.BaseSearch(), new SearchQueries());
            //action
            agroSearchOperation.DeleteElementsWithRelatedElementExceptId(EntityRelated.DOSES, EntityRelated.PRODUCT, ConstantGuids.Value[1], ConstantGuids.Value[2]);


            var queries = agroSearchOperation.Queried;

            // Elimina todas las dosis excepto la que contiene el id 2, y el producto con el id 1.
            Assert.Contains(queries["DeleteElementsWithRelatedElementExceptId"], s => s.Equals($"index eq 6 and  id ne '{ConstantGuids.Value[2]}' and rel/any(elementId: elementId/index eq 12 and elementId/id eq '{ConstantGuids.Value[1]}')"));
            //assert

        }





    }


}
