using Cosmonaut;
using Cosmonaut.Attributes;
using System;
using trifenix.agro.attr;
using trifenix.agro.enums.model;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.db.model.orders {

    [SharedCosmosCollection("agro", "ExecutionOrderStatus")]
    [ReferenceSearchHeader(EntityRelated.EXECUTION_ORDER_STATUS, Kind = EntityKind.CUSTOM_ENTITY)]
    public class ExecutionOrderStatus : DocumentBase, ISharedCosmosEntity {
        public override string Id { get; set; }        
        
        public DateTime Created { get; set; }  // fecha de creacion     

        [EnumSearch(EnumRelated.EXECUTION_STATUS)]
        public ExecutionStatus ExecutionStatus { get; set; } // etapas del proceso

        [EnumSearch(EnumRelated.FINISH_STATUS)]
        public FinishStatus FinishStatus { get; set; }

        [EnumSearch(EnumRelated.CLOSED_STATUS)]
        public ClosedStatus ClosedStatus { get; set; }

        [StringSearch(StringRelated.GENERIC_COMMENT)]
        public string Comment { get; set; }

        [ReferenceSearch(EntityRelated.EXECUTION_ORDER)]
        public string IdExecutionOrder { get; set; }
    }

}