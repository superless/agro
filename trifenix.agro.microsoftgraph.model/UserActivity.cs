using System;
using System.Collections.Generic;

namespace trifenix.agro.microsoftgraph.model
{
    public class User
    {
        private List<string> _roleName;

        public string Name { get; }
        public string Email { get; }
        public List<string> RoleName
        {
            get
            {
                _roleName = _roleName ?? new List<string>();
                return _roleName;
            }
            set { _roleName = value; }
        }
        public User(string name, string email, List<string> roleName)
        {
            Name = name;
            Email = email;
            RoleName = roleName;
        }
    }
    public class UserActivity
    {
        public DateTime Date { get; }
        public User User { get; }

        public UserActivity(DateTime date, User user)
        {
            Date = date;
            User = user;
        }
    }
}