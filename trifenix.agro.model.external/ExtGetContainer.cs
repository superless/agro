using System;
using trifenix.agro.enums;

namespace trifenix.agro.model.external
{
    /// <summary>
    /// Contenedor para respuestas get
    /// </summary>
    /// <typeparam name="T">Tipo de elemento a recuperar</typeparam>
    public class ExtGetContainer<T>
    {
        /// <summary>
        /// Status del resultado
        /// Error, Success o emptyResults
        /// </summary>
        public ExtGetDataResult StatusResult { get; set; }

        /// <summary>
        /// Elemento a recuperar
        /// </summary>
        public T Result { get; set; }

        
        /// <summary>
        /// mensaje de error, si existe.
        /// </summary>
        
        public string ErrorMessage { get; set; }

    }

    /// <summary>
    /// Contenedor para respuestas Get cuando retorna un error
    /// </summary>
    /// <typeparam name="T">Tipo de Elemento a recuperar</typeparam>
    public class ExtGetErrorContainer<T> : ExtGetContainer<T>
    {
        /// <summary>
        /// Excepción capturada.
        /// </summary>
        public Exception InternalException { get; set; }

        /// <summary>
        /// Obtiene el error que debe mostrar al usuario final, es decir la misma, pero sin la excepción interna.
        /// </summary>
        public ExtGetContainer<T> GetBase => new ExtGetContainer<T> {
            ErrorMessage = ErrorMessage,
            Result = Result,
            StatusResult = StatusResult

        };


    }



}
