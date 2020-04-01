using res.model;
using res.model.booleans;
using res.model.dates;
using res.model.doubles;
using res.model.entities;
using res.model.enums;
using res.model.geos;
using res.model.nums;
using res.model.strings;
using System;
using System.Resources;
using trifenix.agro.enums.searchModel;
using trifenix.agro.search.model;
using trifenix.agro.search.model.reflection;

namespace res.core
{
    public static class ResourceExtension
    {
        public static EntitySearchDisplayInfo ResourceModel(Related related, int index) {

            var indexName = GetName(related, index);
            var rm = GetResourceCollection(related);

            var info = new EntitySearchDisplayInfo {
                Title = new ResourceManager(rm.Titles).GetString(indexName),
                Column = rm.Columns == null ? null: new ResourceManager(rm.Columns).GetString(indexName),
                Description = new ResourceManager(rm.Descriptions).GetString(indexName),
                ShortName =  new ResourceManager(rm.ShortNames).GetString(indexName),
            };
            if (info.Title == null && info.Column == null && info.Description == null || info.ShortName == null) return null;

            return info;
        }


        public static string GetName(Related related, int index) {
            switch (related)
            {
                case Related.REFERENCE:                    
                case Related.LOCAL_REFERENCE:
                    return Enum.GetName(typeof(EntityRelated), index);
                case Related.STR:                    
                case Related.SUGGESTION:
                    return Enum.GetName(typeof(StringRelated), index);
                case Related.NUM64:                    
                case Related.NUM32:
                    return Enum.GetName(typeof(NumRelated), index);
                case Related.DBL:
                    return Enum.GetName(typeof(DoubleRelated), index);
                case Related.BOOL:
                    return Enum.GetName(typeof(BoolRelated), index);
                case Related.GEO:
                    return Enum.GetName(typeof(GeoRelated), index);
                case Related.ENUM:
                    return Enum.GetName(typeof(EnumRelated), index);
                default:
                    return Enum.GetName(typeof(DateRelated), index);
                
            }

        }

        public static ResourceModel GetResourceCollection(Related related)
        {
            switch (related)
            {
                case Related.REFERENCE:
                case Related.LOCAL_REFERENCE:

                    return new ResourceModel
                    {
                        Columns = null,
                        Titles = typeof(entity_titles),
                        Descriptions = typeof(entity_descriptions),
                        ShortNames = typeof(entity_snames)
                    };
                case Related.STR:
                case Related.SUGGESTION:
                    return new ResourceModel
                    {
                        Columns = typeof(string_columns),
                        Titles = typeof(string_titles),
                        Descriptions = typeof(string_descriptions),
                        ShortNames = typeof(string_snames)
                    };
                case Related.NUM32:
                case Related.NUM64:
                    return new ResourceModel
                    {
                        Columns = typeof(num_columns),
                        Titles = typeof(num_titles),
                        Descriptions = typeof(num_descriptions),
                        ShortNames = typeof(num_snames)
                    };
                case Related.DBL:
                    return new ResourceModel
                    {
                        Columns = typeof(double_columns),
                        Titles = typeof(double_titles),
                        Descriptions = typeof(double_descriptions),
                        ShortNames = typeof(double_snames)
                    };
                case Related.BOOL:
                    return new ResourceModel
                    {
                        Columns = typeof(boolean_columns),
                        Titles = typeof(boolean_titles),
                        Descriptions = typeof(boolean_descriptions),
                        ShortNames = typeof(boolean_snames)
                    };

                case Related.GEO:
                    return new ResourceModel
                    {
                        Columns = typeof(geo_columns),
                        Titles = typeof(geo_titles),
                        Descriptions = typeof(geo_descriptions),
                        ShortNames = typeof(geo_snames)
                    };
                case Related.ENUM:
                    return new ResourceModel
                    {
                        Columns = typeof(enum_columns),
                        Titles = typeof(enum_titles),
                        Descriptions = typeof(enum_descriptions),
                        ShortNames = typeof(enum_snames)
                    };

                default:
                    return new ResourceModel
                    {
                        Columns = typeof(date_columns),
                        Titles = typeof(date_titles),
                        Descriptions = typeof(date_descriptions),
                        ShortNames = typeof(date_snames)
                    };
            }

        }

    }

    
}
