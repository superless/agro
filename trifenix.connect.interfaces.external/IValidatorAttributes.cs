using System.Threading.Tasks;
using trifenix.connect.entities.cosmos;
using trifenix.connect.input;
using trifenix.connect.interfaces.db.cosmos;
using trifenix.connect.mdm.entity_model;

namespace trifenix.connect.interfaces.external
{
    /// <summary>
    /// Interface para validar elementos genéricos,
    /// generalmente dependiendo de los atributos que tenga
    /// </summary>
    public interface IValidatorAttributes<T> where T : InputBase 
    {

        /// <summary>
        /// Valida un elemento
        /// </summary>
        /// <typeparam name="T">Elemento de entrada</typeparam>
        /// <typeparam name="T2">Elemento en la base de datos</typeparam>
        /// <param name="elemento">Elemento a validar</param>
        /// <returns>True si validacion es correcta y una colección de mensajes en caso de no ser válido</returns>
        Task<ResultValidate> Valida(T elemento);

        IExistElement GetExistElement();

    }
    public class ResultValidate {
        public string[] Messages { get; set; }

        public bool Valid { get; set; }

    }
}
