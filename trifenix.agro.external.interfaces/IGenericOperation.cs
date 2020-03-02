using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;

namespace trifenix.agro.external.interfaces
{
    public interface IGenericOperation<T,T2> : IGenericBaseOperation<T> where T:DocumentBase where T2 : InputBase
    {
        Task<ExtPostContainer<string>> Save(T2 input);
    }

    public interface IGenericFullReadOperation<T,T2> : IGenericOperation<T,T2> where T : DocumentBase where T2: InputBase
    {
        Task<ExtGetContainer<List<T>>> GetElements();
    }


    public interface IGenericBaseOperation<T> where T : DocumentBase    {
        

        Task<ExtGetContainer<T>> Get(string id);
    }

}
