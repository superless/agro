using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace trifenix.connect.agro.interfaces.external.util
{
    public interface IBuildIndex<T>
    {

        Task GenerateIndex(IAgroManager<T> agro);

        Task RegenerateIndex(IAgroManager<T> agro);
    }
}
