using System;

namespace trifenix.agro.microsoftgraph.model{
    public class UserActivity
    {
        public DateTime Date { get; }
        public UserApplicator User { get; }

        public UserActivity(DateTime date, UserApplicator user)
        {
            Date = date;
            User = user;
        }
    }
}