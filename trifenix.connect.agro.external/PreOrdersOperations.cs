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
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro.external
{
    public class PreOrdersOperations<T> : MainOperation<PreOrder, PreOrderInput,T>, IGenericOperation<PreOrder, PreOrderInput> {
        private readonly ICommonAgroQueries commonQueries;

        public PreOrdersOperations(IMainGenericDb<PreOrder> repo,  IAgroSearch<T> search, ICommonAgroQueries commonQueries, ICommonDbOperations<PreOrder> commonDb, IValidatorAttributes<PreOrderInput> validator) : base(repo, search, commonDb, validator) {
            this.commonQueries = commonQueries;
        }
    }

}