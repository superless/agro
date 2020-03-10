using System.Threading.Tasks;
using trifenix.agro.db;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;

namespace trifenix.agro.external.operations {

    public abstract class MainReadOperationName<T, T2> : MainReadOperation<T> where T : DocumentBaseName where T2:InputBaseName {

        public MainReadOperationName(IMainGenericDb<T> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<T> commonDb) : base(repo, existElement, search, commonDb) {}

        public async Task<bool> Validate(T2 input) {
            if (string.IsNullOrWhiteSpace(input.Id)) {
                var existsName = await existElement.ExistsWithPropertyValue<T>("Name", input.Name);
                return !existsName;
            }
            var existsId = await existElement.ExistsById<T>(input.Id);
            if (!existsId)
                return false;            
            var existEditName = await existElement.ExistsWithPropertyValue<T>("Name", input.Name, input.Id);
            return !existEditName;
        }

    }
}