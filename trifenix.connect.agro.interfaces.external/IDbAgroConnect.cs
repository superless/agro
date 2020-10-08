using trifenix.connect.agro.interfaces.cosmos;

namespace trifenix.connect.interfaces.external
{
    public interface IDbAgroConnect : IDbConnect {

        

        IDbExistsElements GetDbExistsElements { get; }


    }

}