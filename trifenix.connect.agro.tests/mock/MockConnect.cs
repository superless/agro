using Cosmonaut;
using Moq;
using System.Linq;
using trifenix.connect.agro.external.helper;
using trifenix.connect.agro.interfaces;
using trifenix.connect.agro.interfaces.cosmos;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.entities.cosmos;
using trifenix.connect.input;
using trifenix.connect.interfaces.db.cosmos;
using trifenix.connect.interfaces.external;
using trifenix.connect.interfaces.graph;

namespace trifenix.connect.agro.tests.mock
{

    public enum ConnectCondition { 
        Default = 0,
        INSERT_PRODUCT =1
    }

    public class MockConnect : IDbAgroConnect
    {


        readonly ConnectCondition connectCondition;
        public MockConnect(ConnectCondition connectCondition = ConnectCondition.Default)
        {
            this.connectCondition = connectCondition;
        }
        

        /// <summary>
        /// Mock de consultas agricolas a la base de datos.
        /// </summary>
        public ICommonAgroQueries CommonQueries
        {
            get
            {
                var mock = new Mock<ICommonAgroQueries>();
                // definición de métodos.
                if (connectCondition != ConnectCondition.Default)
                {
                    switch (connectCondition)
                    {
                        case ConnectCondition.Default:
                            break;
                        case ConnectCondition.INSERT_PRODUCT:
                            mock.Setup(s => s.GetActiveDosesIdsFromProductId(It.IsAny<string>()));
                            break;
                        default:
                            break;
                    }
                }


                return mock.Object;
            }
        }

        /// <summary>
        /// Mock a la api de microsoft para gestión de identidades.
        /// </summary>
        public IGraphApi GraphApi
        {
            get
            {
                var mock = new Mock<IGraphApi>();
                // definición de métodos.



                return mock.Object;
            }
        }

        /// <summary>
        /// Mock de operaciones de base de datos, donde residen consultas de existencia.
        /// </summary>
        public IDbExistsElements GetDbExistsElements => MockHelper.GetExistElement();


        

        /// <summary>
        /// Mock de conversiones de base de datos, cosmos usa un método estático para convertir un IQueriable a lista,
        /// para testear se debe mockear.
        /// </summary>
        /// <typeparam name="T">Tipo de base de datos de persistencia</typeparam>
        /// <returns></returns>
        public ICommonDbOperations<T> GetCommonDbOp<T>() where T : DocumentBase
        {
            var mock = new Mock<ICommonDbOperations<T>>();
            // definición de métodos.


            return mock.Object;
        }

        
        /// <summary>
        /// Mock del objeto principal de base de datos
        /// </summary>
        /// <typeparam name="T">Tipo de base de datos de persistencia</typeparam>
        /// <returns>MainGenericDb</returns>
        public IMainGenericDb<T> GetMainDb<T>() where T : DocumentBase
        {
            var mock = new Mock<IMainGenericDb<T>>();
            // definición de métodos.
            

            return mock.Object;
        }


        /// <summary>
        /// Validaciones de elementos input
        /// </summary>
        /// <typeparam name="T_INPUT"></typeparam>
        /// <typeparam name="T_DB"></typeparam>
        /// <param name="isBatch"></param>
        /// <returns></returns>
        public IValidatorAttributes<T_INPUT> GetValidator<T_INPUT, T_DB>()
            where T_INPUT : InputBase
            where T_DB : DocumentBase
        {
            return new MainValidator<T_DB, T_INPUT>(MockHelper.GetExistElement());
        }
    }
}
