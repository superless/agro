using System;
using System.Collections.Generic;
using System.Text;
using trifenix.agro.enums;

namespace trifenix.agro.db.interfaces.common
{
    public interface IQueries
    {
        string Get(DbQuery query);
    }
}
