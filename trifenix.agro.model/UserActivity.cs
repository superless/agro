using Cosmonaut;
using Cosmonaut.Attributes;
using System;
using trifenix.agro.enums;

namespace trifenix.agro.db.model.agro
{


    [SharedCosmosCollection("agro", "UserActivity")]
    public class UserActivity : DocumentBase, ISharedCosmosEntity
    {

        public override string Id { get; set; }


        public DateTime Date { get; set; }
        public User User { get; set; }

        public UserActivityAction Action { get; set; }

        public string EntityName { get; set; }

        public string IdEntity { get; set; }

        

        


    }
}