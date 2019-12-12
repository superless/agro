using Cosmonaut;
using Cosmonaut.Attributes;
using System.Collections.Generic;
using trifenix.agro.db.model.agro.local;
using trifenix.agro.microsoftgraph.model;

namespace trifenix.agro.db.model.agro
{

    [SharedCosmosCollection("agro", "OrderFolder")]
    public class OrderFolder : DocumentBase, ISharedCosmosEntity
    {
        public override string Id { get; set; }

        public PhenologicalEvent PhenologicalEvent { get; set; }

        public UserInfo Creator { get; set; }

        private List<UserInfo> _modifyBy;
        public List<UserInfo> ModifyBy
        {
            get
            {
                _modifyBy = _modifyBy ?? new List<UserInfo>();
                return _modifyBy;
            }
            set { _modifyBy = value; }
        }

        public ApplicationTarget ApplicationTarget { get; set; }

        public Specie Specie { get; set; }

        public Rootstock Rootstock { get; set; }

        public IngredientCategory Category { get; set; }

        public LocalIngredient Ingredient { get; set; }

        public PhenologicalStage Stage { get; set; }

        public string SeasonId { get; set; }











    }
}
