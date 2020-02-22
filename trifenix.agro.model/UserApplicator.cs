namespace trifenix.agro.db.model {
    public class UserApplicator : User {

        public UserApplicator() : base() {
            CosmosEntityName = "User";
        }

        public Nebulizer Nebulizer { get; set; }
        public Tractor Tractor { get; set; }

    }
}