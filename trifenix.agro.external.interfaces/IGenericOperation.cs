using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;

namespace trifenix.agro.external.interfaces {

    public interface IGenericOperation<T,T2> where T : DocumentBase where T2 : InputBase {

        Task<ExtGetContainer<T>> Get(string id);
        Task<ExtGetContainer<List<T>>> GetElements();
        Task Validate(T2 input, bool isBatch);
        Task<ExtPostContainer<string>> Save(T entity);
        Task<ExtPostContainer<string>> SaveInput(T2 entityInput, bool isBatch);

    }

}