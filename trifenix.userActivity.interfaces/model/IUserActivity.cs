using System;
using System.Text;

namespace trifenix.userActivity.interfaces.model
{
    public interface IUserActivity
    {

        DateTime Date { get; }

        IUser User { get; }

        State ActivityType { get; set; }

        string EntityName { get; set; }



    }

    public enum State { 
        Create,
        Modified,
        Delete
    }








}
