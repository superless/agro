using System;
using System.Collections.Generic;
using System.Text;

namespace trifenix.connect.interfaces.log
{
    public interface ILogSimpleQuery
    {
        /// <summary>   
        /// registra las consultas por método.
        /// </summary>
        Dictionary<string, List<string>> Queried { get; }
    }
}
