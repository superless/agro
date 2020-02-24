using System;
using trifenix.agro.enums;

namespace trifenix.agro.model.external.Input
{
    public class UserActivityInput : InputBase
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }

        public UserActivityAction Action { get; set; }

        public string EntityName { get; set; }

        public string IdEntity { get; set; }
    }
}
