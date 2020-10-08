using trifenix.connect.agro.model_queries;

namespace trifenix.connect.agro.interfaces.cosmos
{
    public interface IQueries
    {
        string Get(DbQuery query);
    }
}
