using System.Threading.Tasks;
using trifenix.agro.db;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;

namespace trifenix.agro.external.operations
{
    public abstract class MainReadOperationName<T, T2> : MainReadOperation<T> where T : DocumentBaseName where T2:InputBaseName
    {
        public MainReadOperationName(IMainGenericDb<T> repo, IExistElement existElement, IAgroSearch search) : base(repo, existElement, search)
        {
        }

        

        public async Task<bool> Validate(T2 input)
        {
            if (string.IsNullOrWhiteSpace(input.Id))
            {
                var existsName = await existElement.ExistsElement<T>("Name", input.Name);
                return !existsName;
            }

            var existsId = await existElement.ExistsElement<T>(input.Id);
            if (!existsId) return false;            
            var existEditName = await existElement.ExistsEditElement<T>(input.Id, "Name", input.Name);
            return !existEditName;

        }
    }
}
