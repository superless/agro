using Cosmonaut.Attributes;
using System;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.model.local {
    [SharedCosmosCollection("agro", "UserActivity")]
    public class UserActivity {
        public DateTime Date { get; }
        public User User { get; }
        public string EntityName { get; set; }
        public string IdEntity { get; set; }

        public UserActivity(DateTime date, UserApplicator user) {
            Date = date;
            User = user;
        }

    }
}