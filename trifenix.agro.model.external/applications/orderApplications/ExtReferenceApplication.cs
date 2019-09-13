using System.Collections.Generic;
using trifenix.agro.db.model.enforcements.ApplicationOrders;
using trifenix.agro.db.model.enforcements.Applications;

namespace trifenix.agro.model.external.applications
{
    public abstract class ExtReferenceApplication
    {
        public string UniqueId { get; set; }

        public long TaskID { get; set; }

        public string TaskName { get; set; }

        public int Duration { get; set; }


        public ApplicationPurpose ApplicationPurpose { get; set; }

        private List<ApplicationInField> _applicationField;

        public List<ApplicationInField> ApplicationField
        {
            get {
                _applicationField = _applicationField ?? new List<ApplicationInField>();
                return _applicationField; }
            set { _applicationField = value; }
        }


    }

}
