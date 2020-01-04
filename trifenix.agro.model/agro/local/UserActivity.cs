using System;

namespace trifenix.agro.db.model.agro.local
{
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