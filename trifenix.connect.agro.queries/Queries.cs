using trifenix.connect.agro.interfaces.cosmos;
using trifenix.connect.agro.model_queries;

namespace trifenix.connect.agro.queries
{
    /// <summary>
    /// Consultas a la base de datos
    /// </summary>
    public class Queries : IQueries {
        public string Get(DbQuery query) {
            switch (query) {
               
                /// <summary>
                /// Obtener la abreviación de una especie mediante su id
                /// </summary>
                case DbQuery.SPECIEABBREVIATION_FROM_SPECIEID:
                    return QueryRes.SPECIEABBREVIATION_FROM_SPECIEID;

                /// <summary>
                /// Obtener el id de una especie segun el id de su variedad
                /// </summary>
                case DbQuery.SPECIEID_FROM_VARIETYID:
                    return QueryRes.SPECIEID_FROM_VARIETYID;

                /// <summary>
                /// Obtener el id de una variedad segun el id de su cuartel
                /// </summary>
                case DbQuery.VARIETYID_FROM_BARRACKID:
                    return QueryRes.VARIETYID_FROM_BARRACKID;

                /// <summary>
                /// Obtener el id de un cuartel segun el id de la orden
                /// </summary>
                case DbQuery.IDBARRACK_FROM_ORDERID:
                    return QueryRes.IDBARRACK_FROM_ORDERID;

                /// <summary>
                /// El mail del usuario segun su rol
                /// </summary>
                case DbQuery.MAILUSERS_FROM_ROLES:
                    return QueryRes.MAILUSERS_FROM_ROLES;

                /// <summary>
                /// Obtener el id de una temporada segun el id de su cuartel
                /// </summary>
                case DbQuery.SEASONID_FROM_BARRACKID:
                    return QueryRes.SEASONID_FROM_BARRACKID;

                /// <summary>
                /// Obtener el id de usuario segun su IDAAD
                /// </summary>
                case DbQuery.USERID_FROM_IDAAD:
                    return QueryRes.USERID_FROM_IDAAD;

                /// <summary>
                /// Contar por ID
                /// </summary>
                case DbQuery.COUNT_BY_ID:
                    return QueryRes.COUNT_BY_ID;

                /// <summary>
                /// Contar por nombre
                /// </summary>
                case DbQuery.COUNT_BY_NAMEVALUE:
                    return QueryRes.COUNT_BY_NAMEVALUE;

                /// <summary>
                /// Contar por nombre ignorando el id
                /// </summary>
                case DbQuery.COUNT_BY_NAMEVALUE_AND_NOID:
                    return QueryRes.COUNT_BY_NAMEVALUE_AND_NOID;

                /// <summary>
                /// Contar ordenes de aplicacion o ejecucion por id de la dosis
                /// </summary>
                case DbQuery.COUNT_EXECUTION_OR_ORDERS_BY_DOSESID:
                    return QueryRes.COUNT_EXECUTION_OR_ORDERS_BY_DOSESID;

                /// <summary>
                /// Contar dosis por id del producto
                /// </summary>
                case DbQuery.COUNT_DOSES_BY_PRODUCTID:
                    return QueryRes.COUNT_DOSES_BY_PRODUCTID;

                /// <summary>
                /// Obtener el número máximo de dosis correlativas mediante el id de un producto
                /// </summary>
                case DbQuery.MAXCORRELATIVE_DOSES_BY_PRODUCTID:
                    return QueryRes.MAXCORRELATIVE_DOSES_BY_PRODUCTID;

                /// <summary>
                /// Obtener el número correlativo mediante id de una dosis
                /// </summary>
                case DbQuery.CORRELATIVE_FROM_DOSESID:
                    return QueryRes.CORRELATIVE_FROM_DOSESID;

                /// <summary>
                /// Obtener las dosis en default mediante el id del producto
                /// </summary>
                case DbQuery.DEFAULTDOSESID_BY_PRODUCTID:
                    return QueryRes.DEFAULTDOSESID_BY_PRODUCTID;

                /// <summary>
                /// Obtener las dosis activas mediante el id del producto
                /// </summary>
                case DbQuery.ACTIVEDOSESIDS_FROM_PRODUCTID:
                    return QueryRes.ACTIVEDOSESIDS_FROM_PRODUCTID;

                /// <summary>
                /// Obtener el nombre mediante el id
                /// </summary>
                case DbQuery.NAME_BY_ID:
                    return QueryRes.NAME_BY_ID;
                default:
                    return "";
            }
        }

    }
}