using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.util.exceptions;

namespace trifenix.agro.util {
    public static class LinqHelper {

        /// <summary>
        /// desde una lista con identificadores, busca cada elemento con getElement, si no encuentra el elemento lanza excepción
        /// </summary>
        /// <typeparam name="T">Tipo de elemento</typeparam>
        /// <param name="list"></param>
        /// <param name="getElement"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async Task<List<T>> SelectElement<T>(this IEnumerable<string> list, Func<string, Task<T>> getElement, string message) {
            if (list == null) throw new GenericException(message);
            if (list.Any(s => s == null)) throw new GenericException(message);
            var listLocal = new List<T>();
            foreach (var item in list) {
                var element = await getElement(item);
                if (element == null) throw new GenericException(message);
                listLocal.Add(element);
            }
            return listLocal;
        }
    }
}