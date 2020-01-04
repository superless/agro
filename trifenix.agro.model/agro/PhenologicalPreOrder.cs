using Cosmonaut;
using Cosmonaut.Attributes;
using System;
using System.Collections.Generic;
using trifenix.agro.db.model.agro.local;

namespace trifenix.agro.db.model.agro
{
    [SharedCosmosCollection("agro", "PhenologicalPreOrder")]
    public class PhenologicalPreOrder : DocumentBase, ISharedCosmosEntity
    {
        public override string Id { get; set; }

        public string Name { get; set; }

        public string SeasonId { get; set; }

        public string OrderFolderId { get; set; }

        public UserActivity Creator { get; set; }

        private List<UserActivity> _modifyBy;
        public List<UserActivity> ModifyBy
        {
            get
            {
                _modifyBy = _modifyBy ?? new List<UserActivity>();
                return _modifyBy;
            }
            set { _modifyBy = value; }
        }

        private List<string> _barracksId;

        public List<string> BarracksId
        {
            get {
                _barracksId = _barracksId ?? new List<string>();
                return _barracksId; }
            set { _barracksId = value; }
        }

        public DateTime Created { get; set; }





    }
}


