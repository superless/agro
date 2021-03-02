using trifenix.connect.agro.interfaces.db;
using trifenix.connect.interfaces.external;

namespace trifenix.connect.agro.interfaces.external
{
    /// <summary>
    /// Operaciones de base de datos para agricola.
    /// </summary>
    public interface IDbAgroConnect : IDbConnect {

        /// <summary>
        /// Operaciones de consultas a base de datos agrícola.
        /// </summary>
        ICommonAgroQueries CommonQueries { get; }

        /// <summary>
        /// Obtiene existencias en base de datos.
        /// </summary>
        IDbExistsElements GetDbExistsElements { get; }


    }

}