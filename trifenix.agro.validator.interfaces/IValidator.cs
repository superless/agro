using System;
using System.Threading.Tasks;

namespace trifenix.agro.validator.interfaces {

    public interface IValidator {

        Task ValidateRecursively<T_Attr>(object obj) where T_Attr : Attribute;

    }

    public interface IValidate {

        Task Validate(object Obj, params object[] Args);

    }

}