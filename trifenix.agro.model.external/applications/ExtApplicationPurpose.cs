using System.Collections.Generic;

namespace trifenix.agro.model.external.applications
{
    public class ExtApplicationPurpose
    {

        public string Suffix => "PA";
        public string UniqueId { get; set; }

        public string TaskName { get; set; }

        public string TaskID { get; set; }



        private List<ExtReferenceApplication> _subTaks;

        public List<ExtReferenceApplication> SubTasks
        {
            get
            {
                _subTaks = _subTaks ?? new List<ExtReferenceApplication>();
                return _subTaks;
            }
            set { _subTaks = value; }
        }

        



    }

}
