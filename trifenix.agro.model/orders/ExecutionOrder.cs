using Cosmonaut;
using Cosmonaut.Attributes;
using System;

namespace trifenix.agro.db.model.agro.orders {

    [SharedCosmosCollection("agro", "ExecutionOrder")]
    public class ExecutionOrder : DocumentBase, ISharedCosmosEntity {

        public override string Id { get; set; }
        public string IdOrder { get; set; }
        public DosesOrder[] DosesOrder { get; set; }
        public DateTime? InitDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string IdUserApplicator { get; set; }
        public string IdNebulizer { get; set; }
        public string IdTractor { get; set; }

    }

}