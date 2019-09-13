using Cosmonaut;
using Cosmonaut.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using trifenix.agro.db.model.enforcements.Applications;

namespace trifenix.agro.db.model.enforcements.ApplicationOrders
{
    
    public class ReferenceApplicationOrder : DocumentBase
    {
       
        public override string Id { get; set; }

        public long TaskId { get; set; }


        


        public string ApplicationName { get; set; }


        public ApplicationPurpose ApplicationPurpose { get; set; }

        private List<ApplicationInField> _applications;

        public List<ApplicationInField> Applications
        {
            get {
                _applications = _applications ?? new List<ApplicationInField>();
                return _applications; }
            set { _applications = value; }
        }

        public int Duration { get; set; }

        public DateTime CreationDate { get; set; }


    }
}
