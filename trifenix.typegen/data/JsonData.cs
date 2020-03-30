using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using trifenix.agro.attr;
using trifenix.agro.db.model;
using trifenix.agro.enums.searchModel;
using trifenix.agro.search.model.ts;
using trifenix.agro.search.operations.util;
using TypeGen.Core.Extensions;
using static trifenix.agro.search.operations.util.AgroHelper;

namespace trifenix.typegen.data
{
    public static class JsonData
    {
        public static ModelDictionary GetModel(IEnumerable<PropertySearchInfo> propertySearchInfos)
        {
            var propByRelatedAndIndex = propertySearchInfos.GroupBy(s => new { s.SearchAttribute.Index, s.SearchAttribute.Related, s.IndexClass }).Select(s => s.FirstOrDefault());


            var boolEnums = GetDescription(typeof(BoolRelated));

            var stringEnum = GetDescription(typeof(StringRelated));

            var doubleEnum = GetDescription(typeof(DoubleRelated));

            var dateEnum = GetDescription(typeof(DateRelated));

            var geoEnum = GetDescription(typeof(GeoRelated));

            var numEnum = GetDescription(typeof(NumRelated));

            var enumEmun = GetDescription(typeof(EnumRelated));

            var enumRelated = GetDescription(typeof(EntityRelated));



            var modelDictionary = new ModelDictionary()
            {
                BoolData = GetDictionaryFromRelated(propByRelatedAndIndex, Related.BOOL, boolEnums),

                StringData = GetDictionaryFromRelated(propByRelatedAndIndex, Related.STR, stringEnum),

                DateData = GetDictionaryFromRelated(propByRelatedAndIndex, Related.DATE, dateEnum),

                DoubleData = GetDictionaryFromRelated(propByRelatedAndIndex, Related.DBL, doubleEnum),

                EnumData = GetEnumDictionaryFromRelated(propByRelatedAndIndex, enumEmun),

                GeoData = GetDictionaryFromRelated(propByRelatedAndIndex, Related.GEO, geoEnum),

                NumData = GetDictionaryFromRelated(propByRelatedAndIndex, Related.NUM32, numEnum),

                RelatedData = GetDictionaryFromRelated(propByRelatedAndIndex, Related.REFERENCE, enumRelated),



            };

            //include suggestion, Refererence Local and num64,
            var suggestions = GetDictionaryFromRelated(propByRelatedAndIndex, Related.SUGGESTION, stringEnum);

            var referenceLocal = GetDictionaryFromRelated(propByRelatedAndIndex, Related.LOCAL_REFERENCE, enumRelated);

            var num64 = GetDictionaryFromRelated(propByRelatedAndIndex, Related.NUM64, numEnum);

            var suggestionNotInString = suggestions.Where(sg => !modelDictionary.StringData.Any(s => s.Key == sg.Key));

            var localNotInReference = referenceLocal.Where(sg => !modelDictionary.RelatedData.Any(s => s.Key == sg.Key));

            var num64NotInNum = num64.Where(sg => !modelDictionary.NumData.Any(s => s.Key == sg.Key));

            if (suggestionNotInString.Any())
            {
                foreach (var item in suggestionNotInString)
                {
                    modelDictionary.StringData.Add(item.Key, item.Value);
                }
            }

            if (localNotInReference.Any())
            {
                foreach (var item in localNotInReference)
                {
                    modelDictionary.RelatedData.Add(item.Key, item.Value);
                }
            }

            if (num64NotInNum.Any())
            {
                foreach (var item in num64NotInNum)
                {
                    modelDictionary.NumData.Add(item.Key, item.Value);
                }
            }




            return modelDictionary;

        }
        public static ModelByIndex GetJsonData() {

            // get assemblu
            var assembly = Assembly.GetAssembly(typeof(Barrack));

            //get model types from namespace
            var modelTypes = assembly.GetLoadableTypes()
            .Where(x => x.FullName.StartsWith("trifenix.agro.db.model"));

            // get property infos
            var propSearchinfos = modelTypes.Where(s => s.GetTypeInfo().GetCustomAttribute<ReferenceSearchAttribute>(true) != null).SelectMany(GetPropertySearchInfo).ToList();

            var grpIndexes = propSearchinfos.GroupBy(s => s.IndexClass).Select(s=>s.FirstOrDefault()) ;



            var indexes = grpIndexes.ToDictionary(i => i.IndexClass, g => GetModel(propSearchinfos.Where(s => s.IndexClass == g.IndexClass)));


            return new ModelByIndex {
                Indexes = indexes
            };
            
            // group by index and related, get the first element
            

        }



 
        private static Dictionary<int, DefaultDictionary> GetDictionaryFromRelated(IEnumerable<PropertySearchInfo> propSearchInfos, Related related, Dictionary<int, string> enumDescription) {

            return propSearchInfos.Where(s => s.SearchAttribute.Related == related).ToDictionary(s => s.SearchAttribute.Index, g => new DefaultDictionary
            {
                NameProp = g.Name,
                isArray = g.Array,
                Description = enumDescription[g.SearchAttribute.Index]
            });
        }

        private static Dictionary<int, EnumDictionary> GetEnumDictionaryFromRelated(IEnumerable<PropertySearchInfo> propSearchInfos, Dictionary<int, string> enumDescription)
        {

            return propSearchInfos.Where(s => s.SearchAttribute.Related == Related.ENUM).ToDictionary(s => s.SearchAttribute.Index, g => new EnumDictionary
            {
                NameProp = g.Name,
                isArray = g.Array,
                Description = enumDescription[g.SearchAttribute.Index],
                EnumData = g.Enums
            });
        }




    }
}
