using Cosmonaut;
using Cosmonaut.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace trifenix.agro.db.model.agro
{
    [SharedCosmosCollection("agro", "PhenologicalPreOrder")]
    public class PhenologicalPreOrder : DocumentBase, ISharedCosmosEntity
    {
        public override string Id { get; set; }

        public string Name { get; set; }

        public string SeasonId { get; set; }

        public string OrderFolderId { get; set; }

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


