namespace trifenix.agro.db.model.agro
{
    public class UserApplicator : User
    {
        public UserApplicator() : base()
        {
            CosmosEntityName = "User";
        }
        public string IdNebulizer { get; set; }

        public string IdTractor { get; set; }

    }
}
