using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db;
using trifenix.agro.db.interfaces;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;

namespace trifenix.agro.external.interfaces {

// <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Agro-Dev

    public interface IGenericOperation<T,T2> : IGenericBaseOperation<T> where T : DocumentBase where T2 : InputBase {
        Task<ExtPostContainer<string>> Save(T2 input);

        Task Remove(string id);
    }

  

    public interface IGenericBaseOperation<T> where T : DocumentBase {
// ========================================================================
//     public interface IGenericOperation<T,T2> where T : DocumentBase where T2 : InputBase {
// >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> Script_PoblarDB

        Task<ExtGetContainer<T>> Get(string id);
        Task<ExtGetContainer<List<T>>> GetElements();
        Task Validate(T2 input, bool isBatch);
        Task<ExtPostContainer<string>> Save(T entity);
        Task<ExtPostContainer<string>> SaveInput(T2 entityInput, bool isBatch);

        IMainGenericDb<T> Store { get; }
    }

}