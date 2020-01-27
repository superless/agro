using Cosmonaut;
using Cosmonaut.Attributes;
using System;
using System.Collections.Generic;
using trifenix.agro.db.model.agro.local;

namespace trifenix.agro.db.model.agro.orders
{
    [SharedCosmosCollection("agro", "ExecutionOrder")]
    public class ExecutionOrder : DocumentBase, ISharedCosmosEntity {
        /// <summary>
        /// Identificador de la entidad
        /// </summary>
        public override string Id { get; set; }
        public string Name { get; set; }
        public ApplicationOrder Order { get; set; }

        private List<ProductToApply> _productToApply;

        public List<ProductToApply> ProductToApply
        {
            get {
                _productToApply = _productToApply ?? new List<ProductToApply>();
                return _productToApply; }
            set { _productToApply = value; }
        }

        private ExecutionStatus _executionStatus;
        public ExecutionStatus ExecutionStatus {
            get => _executionStatus;
            set { 
                _executionStatus = value;
                if ((int)_executionStatus == 1){
                    InitDate = Order.InitDate;
                    EndDate = Order.EndDate;
                }
            }
        }

        private Comments[] _statusInfo;
        /// <summary>
        /// Array que almacena el usuario, la fecha y un comentario para cada estado de ejecucion, indexado segun la enumeracion de estos.
        /// </summary>
        public Comments[] StatusInfo
        {
            get {
                _statusInfo = _statusInfo ?? new Comments[Enum.GetValues(typeof(ExecutionStatus)).Length];
                return _statusInfo;
            }
            set { _statusInfo = value; }
        }

        public FinishStatus FinishStatus;

        public ClosedStatus ClosedStatus;

        public DateTime InitDate { get; set; }
        public DateTime EndDate { get; set; }

        public UserApplicator UserApplicator;
        public Nebulizer Nebulizer { get; set; }
        public Tractor Tractor { get; set; }

        private List<Comments> _comments;
        public List<Comments> Comments
        {
            get
            {
                _comments = _comments ?? new List<Comments>();
                return _comments;
            }
            set { _comments = value; }
        }

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

    }

    public class Comments {

        public UserActivity _userActivity;
        public string _commentary;

        public Comments(UserActivity UserActivity, string Commentary = null) {
            _userActivity = UserActivity;
            _commentary = Commentary;
        }

    }

    public class ProductToApply {
        public Product Product { get; set; }
        public double QuantityByHectare { get; set; }
    }

    public enum FinishStatus {
        Successful = 1,
        InComplete = 2,
        Cancelled = 3,
        Rescheduled = 4
    }

    public enum ClosedStatus {
        Successful = 1,
        Failed = 2
    }

    public enum ExecutionStatus {
        Planification = 0,
        InProcess = 1,
        EndProcess = 2,
        Closed = 3
    }
}