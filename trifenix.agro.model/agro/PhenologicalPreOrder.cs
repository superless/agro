using Cosmonaut;
using Cosmonaut.Attributes;
using System;
using System.Collections.Generic;
using trifenix.agro.microsoftgraph.model;

namespace trifenix.agro.db.model.agro
{
    [SharedCosmosCollection("agro", "PhenologicalPreOrder")]
    public class PhenologicalPreOrder : DocumentBase, ISharedCosmosEntity
    {
        public override string Id { get; set; }

        public string Name { get; set; }

        public string SeasonId { get; set; }

        public string OrderFolderId { get; set; }

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


