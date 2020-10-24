using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using trifenix.connect.entities.cosmos;

namespace trifenix.connect.interfaces.db.cosmos
{

    /// <summary>
    /// CosmosDb usa un método static que transforma el resultado como IQueryable, en una lista con resultados.
    /// Para poder testearlo usamos esta interface, lo que permite obviar el iqueriable y retornar el resultado que deseemos
    /// o leer lo que deseemos del IQueriable formado por CosmosDb.
    /// Esta implementación deberá estar presente en otras bases de datos, incluso aunque tengan que pasarla a ToList.
    /// </summary>
    /// <typeparam name="T">Elemento de una base de datos</typeparam>
    public interface ICommonDbOperations<T> where T:DocumentBase {

        /// <summary>
        /// Retorna una lista de elementos desde un IQueryable.
        /// </summary>
        /// <param name="list">IQueryable a convertir</param>
        /// <returns>Listado de elementos</returns>
        Task<List<T>> TolistAsync(IQueryable<T> list);

        /// <summary>
        /// El primer elemento que encuentre desde una lista filtrada por el parámetro de entrada (predicate).
        /// </summary>
        /// <param name="list">IQueryable a convertir</param>
        /// <param name="predicate">filtro de busqueda</param>
        /// <returns></returns>
        Task<T> FirstOrDefaultAsync(IQueryable<T> list, Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Este método usa un IQueriable y lo convierte en un resultado determinado por la página que determine y que es cálculado
        /// de acuerdo a el número total de elentos que se perniten en cada página.
        /// este método es bastante dependiente de Cosmonaut, dado que este usa un Iqueriable para convertir en una colección páginada.
        /// </summary>
        /// <param name="list">IQueriable a consultar</param>
        /// <param name="page">página a buscar</param>
        /// <param name="totalElementsOnPage">total de elementos en página</param>
        /// <returns></returns>
        IQueryable<T> WithPagination(IQueryable<T> list, int page, int totalElementsOnPage);
    }
}