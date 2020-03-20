using System;
using System.Collections.Generic;
using System.Text;
using trifenix.agro.db.applicationsReference.common.res;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.enums;

namespace trifenix.agro.db.applicationsReference.common
{
    public class Queries : IQueries
    {
        public string Get(DbQuery query)
        {
            switch (query)
            {
                case DbQuery.SPECIEABBREVIATION_FROM_SPECIEID:
                    return QueryRes.SPECIEABBREVIATION_FROM_SPECIEID;
                case DbQuery.SPECIEID_FROM_VARIETYID:
                    return QueryRes.SPECIEID_FROM_VARIETYID;
                case DbQuery.VARIETYID_FROM_BARRACKID:
                    return QueryRes.VARIETYID_FROM_BARRACKID;
                case DbQuery.IDBARRACK_FROM_ORDERID:
                    return QueryRes.IDBARRACK_FROM_ORDERID;
                case DbQuery.MAILUSERS_FROM_ROLES:
                    return QueryRes.MAILUSERS_FROM_ROLES;
                case DbQuery.SEASONID_FROM_BARRACKID:
                    return QueryRes.SEASONID_FROM_BARRACKID;
                case DbQuery.USERID_FROM_IDAAD:
                    return QueryRes.USERID_FROM_IDAAD;
                case DbQuery.COUNT_BY_ID:
                    return QueryRes.COUNT_BY_ID;
                case DbQuery.COUNT_BY_NAMEVALUE:
                    return QueryRes.COUNT_BY_NAMEVALUE;
                case DbQuery.COUNT_BY_NAMEVALUE_AND_NOID:
                    return QueryRes.COUNT_BY_NAMEVALUE_AND_NOID;
                case DbQuery.COUNT_EXECUTION_OR_ORDERS_BY_DOSESID:
                    return QueryRes.COUNT_EXECUTION_OR_ORDERS_BY_DOSESID;
                case DbQuery.COUNT_DOSES_BY_PRODUCTID:
                    return QueryRes.COUNT_DOSES_BY_PRODUCTID;
                case DbQuery.MAXCORRELATIVE_DOSES_BY_PRODUCTID:
                    return QueryRes.MAXCORRELATIVE_DOSES_BY_PRODUCTID;
                case DbQuery.CORRELATIVE_FROM_DOSESID:
                    return QueryRes.CORRELATIVE_FROM_DOSESID;
                case DbQuery.DEFAULTDOSESID_BY_PRODUCTID:
                    return QueryRes.DEFAULTDOSESID_BY_PRODUCTID;
                case DbQuery.ACTIVEDOSESIDS_FROM_PRODUCTID:
                    return QueryRes.ACTIVEDOSESIDS_FROM_PRODUCTID;
                case DbQuery.NAME_BY_ID:
                    return QueryRes.NAME_BY_ID;
                    break;
                default:
                    return "";
            }
        }
    }
}
