using Cosmonaut.Attributes;
using System;
using trifenix.userActivity.interfaces.model;

namespace trifenix.agro.db.model.agro.local
{


    [SharedCosmosCollection("agro", "UserActivity")]
    public class UserActivity : IUserActivity
    {
        public DateTime Date { get; }
        public IUser User { get; }
        public State ActivityType { get; set; }
        public string EntityName { get; set; }

        public UserActivity(DateTime date, UserApplicator user)
        {
            Date = date;
            User = user;
        }


    }
}