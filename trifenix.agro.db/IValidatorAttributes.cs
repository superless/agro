using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace trifenix.connect.interfaces
{
    /// <summary>
    /// Interface para validar elementos genéricos,
    /// generalmente dependiendo de los atributos que tenga
    /// </summary>
    interface IValidatorAttributes
    {

        //
        Task<bool> Valida<T>(T elemento) where T:Input;

    }
}
