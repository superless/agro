using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;

namespace trifenix.agro.external.interfaces {

    public interface IGenericOperation<T,T2> where T : DocumentBase where T2 : InputBase {

        Task<ExtGetContainer<T>> Get(string id);
        Task<ExtGetContainer<List<T>>> GetElements();
        Task<string> Validate(T2 input);
        Task<ExtPostContainer<string>> Save(T2 input);

    }

}