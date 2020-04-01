using System;
using System.Resources;
using trifenix.agro.enums.searchModel;


namespace trifenix.agro.attr
{


    public class ResourceSearchs {
        public Type Columns { get; set; }

        public Type Descriptions { get; set; }

        public Type ShortNames { get; set; }

        public Type Titles { get; set; }

    }

    public abstract class DisplaySearchAttribute  {
        protected readonly int index;


        public DisplaySearchAttribute(ResourceSearchs resources, ResourceRelated related)
        {
            Resources = resources;
            Related = related;

            

        }

        protected abstract string GetIndexName();


        public ResourceManager ResourceManagerColumns => new ResourceManager(Resources.Columns);
        public ResourceManager ResourceManagerTitles => new ResourceManager(Resources.Titles);
        public ResourceManager ResourceManagerDescriptions => new ResourceManager(Resources.Descriptions);
        public ResourceManager ResourceManagerShortNames => new ResourceManager(Resources.ShortNames);


        public ResourceSearchs Resources { get; set; }


        public ResourceRelated Related { get; }


        public string Title => ResourceManagerTitles.GetString(GetIndexName());

        public string ShortName => ResourceManagerShortNames.GetString(GetIndexName());

        public string Description => ResourceManagerDescriptions.GetString(GetIndexName());

        public string Column => ResourceManagerColumns.GetString(GetIndexName());






    }




}