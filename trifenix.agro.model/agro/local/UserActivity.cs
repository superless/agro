using Cosmonaut.Attributes;
using System;

namespace trifenix.agro.db.model.agro.local
{


    [SharedCosmosCollection("agro", "UserActivity")]
    public class UserActivity
    {
        public DateTime Date { get; }
        public User User { get; }
        
        public string EntityName { get; set; }



        public string IdEntity { get; set; }

        

        public UserActivity(DateTime date, UserApplicator user)
        {
            Date = date;
            User = user;
        }


    }
}