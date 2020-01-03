using Cosmonaut;
using Cosmonaut.Attributes;
using System;
using System.Collections.Generic;
using trifenix.agro.microsoftgraph.model;

namespace trifenix.agro.db.model.agro.orders
{
    [SharedCosmosCollection("agro", "ExecutionOrder")]
    public class ExecutionOrder : DocumentBase, ISharedCosmosEntity {
        /// <summary>
        /// Identificador de la entidad
        /// </summary>
        public override string Id { get; set; }
        public string IdOrder { get; set; }

        public ExecutionStatus ExecutionStatus;

        private DateTime[] _executionStatusDate;
        /// <summary>
        /// Array que almacena la fecha en que inicio cada estado de ejecucion, indexado segun la enumeracion de estos.
        /// </summary>
        public DateTime[] ExecutionStatusDate {
            get {
                _executionStatusDate = _executionStatusDate ?? new DateTime[Enum.GetValues(typeof(ExecutionStatus)).Length];
                return _executionStatusDate;
            }
            set { _executionStatusDate = value; }
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

        public Comments(UserActivity UserActivity, string Commentary)
        {
            _userActivity = UserActivity;
            _commentary = Commentary;
        }

    }

    public enum FinishStatus {
        Successful = 0,
        InComplete = 1,
        Cancelled = 2,
        Rescheduled = 3
    }

    public enum ClosedStatus {
        Successful = 0,
        Failed = 1
    }

    public enum ExecutionStatus {
        Planification = 0,
        InProcess = 1,
        EndProcess = 2,
        Closed = 3
    }
}
