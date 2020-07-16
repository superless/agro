using System.Resources;
using trifenix.connect.agro.resources.helper;
using trifenix.connect.mdm.enums;
using trifenix.connect.mdm.resources;
using trifenix.connect.mdm.ts_model;

namespace trifenix.connect.agro.resources
{
    public class MdmDocs : IMdmDocumentation
    {
        public EntitySearchDisplayInfo GetInfoFromEntity(int index)
        {
            var indexName = ResourcesHelper.GetEntityName(index);
            var rm = ResourcesHelper.GetEntityResourceCollection();


            var info = new EntitySearchDisplayInfo
            {
                Title = new ResourceManager(rm.Titles).GetString(indexName),
                Column = rm.Columns == null ? null : new ResourceManager(rm.Columns).GetString(indexName),
                Description = new ResourceManager(rm.Descriptions).GetString(indexName),
                ShortName = new ResourceManager(rm.ShortNames).GetString(indexName),
            };
            if (info.Title == null && info.Column == null && info.Description == null || info.ShortName == null) return null;

            return info;
        }



        public EntitySearchDisplayInfo GetInfoFromProperty(KindProperty property, int index)
        {
            var indexName = ResourcesHelper.GetName(property, index);
            
            var rm = ResourcesHelper.GetResourceCollection(property);

            var info = new EntitySearchDisplayInfo
            {
                Title = new ResourceManager(rm.Titles).GetString(indexName),
                Column = rm.Columns == null ? null : new ResourceManager(rm.Columns).GetString(indexName),
                Description = new ResourceManager(rm.Descriptions).GetString(indexName),
                ShortName = new ResourceManager(rm.ShortNames).GetString(indexName),
            };
            if (info.Title == null && info.Column == null && info.Description == null || info.ShortName == null) return null;

            return info;
        }
    }
}
