using Cosmonaut;
using Cosmonaut.Attributes;
using System;
using System.Collections.Generic;
using trifenix.agro.db.model.agro.local;
using trifenix.agro.enums;

namespace trifenix.agro.db.model.agro.orders
{
    [SharedCosmosCollection("agro", "ExecutionOrder")]
    public class ExecutionOrder : DocumentBase, ISharedCosmosEntity {
       


        public override string Id { get; set; }

        public string IdOrder { get; set; }

        private List<ProductToApply> _productToApply;

        public List<ProductToApply> ProductToApply
        {
            get {
                _productToApply = _productToApply ?? new List<ProductToApply>();
                return _productToApply; }
            set { _productToApply = value; }
        }


        public ExecutionStatus ExecutionStatus { get; set; }


        public string[] StatusInfo { get; set; }

        public FinishStatus FinishStatus;

        public ClosedStatus ClosedStatus;

        public DateTime? InitDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string IdUserApplicator;
        public string IdNebulizer { get; set; }
        public string IdTractor { get; set; }



        

        public string IdCreator { get; set; }

        
       

    }

    

    public class ProductToApply {
        public string IdProduct { get; set; }
        public double QuantityByHectare { get; set; }
    }

    
}