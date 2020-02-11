using System.Collections.Generic;

namespace trifenix.userActivity.interfaces.model
{
    public interface IUser
    {
        string Id { get; set; }

        string ObjectIdAAD { get; set; }

        string Name { get; set; }

        string Rut { get; set; }

        string Email { get; set; }

        IJob Job { get; set; }

        List<IRole> Roles { get; set; }
    } 





}
