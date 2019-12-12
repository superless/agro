using System;
using System.Collections.Generic;

namespace trifenix.agro.microsoftgraph.model
{
    public class User
    {
        private string _name;
        private string _email;
        private List<string> _roleName;

        public string Name { get => _name; }
        public string Email { get => _email; }
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
            _name = name;
            _email = email;
            _roleName = roleName;
        }
    }
    public class UserInfo
    {
        private DateTime _date;
        private User _user;

        public DateTime Date { get => _date; }
        public User User { get => _user; }

        public UserInfo(DateTime date, User user)
        {
            _date = date;
            _user = user;
        }
    }
}