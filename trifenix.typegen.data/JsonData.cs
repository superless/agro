using res.core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using trifenix.agro.attr;
using trifenix.agro.db.model;
using trifenix.agro.enums.searchModel;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.model.reflection;
using trifenix.agro.search.model.ts;
using trifenix.agro.search.operations.util;
using TypeGen.Core.Extensions;
using static trifenix.agro.util.AttributesExtension;

namespace trifenix.typegen.data {
    public static class JsonData {
        public static EntityMetadata GetModel(IEnumerable<PropertySearchInfo> propertySearchInfos, int index) {
            var propByRelatedAndIndex = propertySearchInfos.GroupBy(s => new {  s.Related, s.IndexClass, s.Index }).Select(s => s.FirstOrDefault());
            var enumEmun = GetDescription(typeof(EnumRelated));
            var modelInfo = ResourceExtension.ResourceModel(KindProperty.REFERENCE, propByRelatedAndIndex.FirstOrDefault().IndexClass);
            var modelDictionary = new EntityMetadata() {
                Index = index,
                Description = modelInfo.Description,
                ShortName = modelInfo.ShortName,
                Title = modelInfo.Title,
                BoolData = GetDictionaryFromRelated(propByRelatedAndIndex, KindProperty.BOOL),
                StringData = GetDictionaryFromRelated(propByRelatedAndIndex, KindProperty.STR),
                DateData = GetDictionaryFromRelated(propByRelatedAndIndex, KindProperty.DATE),
                DoubleData = GetDictionaryFromRelated(propByRelatedAndIndex, KindProperty.DBL),
                EnumData = GetEnumDictionaryFromRelated(propByRelatedAndIndex, enumEmun),
                GeoData = GetDictionaryFromRelated(propByRelatedAndIndex, KindProperty.GEO),
                NumData = GetDictionaryFromRelated(propByRelatedAndIndex, KindProperty.NUM32),
                relData = GetDictionaryFromRelated(propByRelatedAndIndex, KindProperty.REFERENCE),

            };
            var suggestions = GetDictionaryFromRelated(propByRelatedAndIndex, KindProperty.SUGGESTION);
            var num64 = GetDictionaryFromRelated(propByRelatedAndIndex, KindProperty.NUM64);
            var suggestionNotInString = suggestions.Where(sg => !modelDictionary.StringData.Any(s => s.Key == sg.Key));
            var num64NotInNum = num64.Where(sg => !modelDictionary.NumData.Any(s => s.Key == sg.Key));
            var relLocal = GetDictionaryFromRelated(propByRelatedAndIndex, KindProperty.LOCAL_REFERENCE);
            if (suggestionNotInString.Any())
                foreach (var item in suggestionNotInString)
                    modelDictionary.StringData.Add(item.Key, item.Value);
            if (num64NotInNum.Any())
                foreach (var item in num64NotInNum)
                    modelDictionary.NumData.Add(item.Key, item.Value);
            if (relLocal.Any())
                foreach (var item in relLocal)
                    modelDictionary.relData.Add(item.Key, item.Value);

            return modelDictionary;
        }

        

        public static ModelMetaData GetJsonData() {
            // get assembly
            var assembly = Assembly.GetAssembly(typeof(Barrack));
            //get model types from namespace
            var modelTypes = assembly.GetLoadableTypes()
            .Where(x => x.FullName.StartsWith("trifenix.agro.db.model"));
            // get property infos
            var propSearchinfos = modelTypes.Where(s => s.GetTypeInfo().GetCustomAttributes<ReferenceSearchHeaderAttribute>(true).Any()).SelectMany(s=> {

                var infoHeaders = s.GetTypeInfo().GetCustomAttributes<ReferenceSearchHeaderAttribute>(true);

                return infoHeaders.Select(infoHeader => new
                {
                    index = infoHeader.Index,
                    visible = infoHeader.Visible,
                    pathName = infoHeader.PathName,
                    kindEntity = infoHeader.Kind,
                    propInfos = GetPropertySearchInfo(s).Where(a=>a.IndexClass == infoHeader.Index),
                    className = s.Name
                });

            }).ToList();
          
            var modelDict = propSearchinfos.Select(s =>
            {
                var model = GetModel(s.propInfos, s.index);
                model.Visible = s.visible;
                model.PathName = s.pathName;
                model.EntityKind = s.kindEntity;
                model.ClassName = s.className;
                model.AutoNumeric = s.propInfos.Any(a => a.AutoNumeric);
                return model;

            });
            return new ModelMetaData { Indexes = modelDict.ToArray() };
            // group by index and related, get the first element
        }
 
        private static Dictionary<int, PropertyMetadata> GetDictionaryFromRelated(IEnumerable<PropertySearchInfo> propSearchInfos, KindProperty related) {
            var infos = propSearchInfos.Where(s => s.Related == related).ToList();
            return infos.ToDictionary(s => s.Index, g => new PropertyMetadata {
                Visible = g.Visible,
                AutoNumeric = g.AutoNumeric,
                NameProp = char.ToLower(g.Name[0]) + g.Name.Substring(1) ,
                isArray = g.IsEnumerable,
                Info = g.Info,
                Required = g.IsRequired,
                Unique = g.IsUnique,
                HasInput = g.HasInput
            });
        }

        private static Dictionary<int, PropertyMetadadataEnum> GetEnumDictionaryFromRelated(IEnumerable<PropertySearchInfo> propSearchInfos, Dictionary<int, string> enumDescription) {
            return propSearchInfos.Where(s => s.Related == KindProperty.ENUM).ToDictionary(s => s.Index, g => new PropertyMetadadataEnum {
                NameProp = char.ToLower(g.Name[0]) + g.Name.Substring(1),
                isArray = g.IsEnumerable,
                Info = g.Info,
                EnumData = g.Enums
            });
        }




        public static PropertySearchInfo[] GetPropertyByIndex(Type type, int index) {
            var searchAttributesProps = type.GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(SearchAttribute), true));
            var elemTypeInput = AgroHelper.GetEntityType((EntityRelated)index, typeof(BarrackInput), "trifenix.agro.model.external.Input");


            var elemTypeInputProps = elemTypeInput.GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(SearchAttribute), true))
                .Select(s=>new { info = s, search = s.GetAttribute<SearchAttribute>(), required = s.GetAttribute<RequiredAttribute>(), unique = s.GetAttribute<UniqueAttribute>() });



            var props = searchAttributesProps.Select(s => {
                var searchAttribute = (SearchAttribute)s.GetCustomAttributes(typeof(SearchAttribute), true).FirstOrDefault();
                var searchAttributeInput = elemTypeInputProps.FirstOrDefault(p => p.search.Index == searchAttribute.Index && p.search.Related == searchAttribute.Related);

                return new PropertySearchInfo {
                    IsEnumerable = IsEnumerableProperty(s),
                    Name = s.Name,
                    Index = searchAttribute.Index,
                    Related = searchAttribute.Related,
                    Enums = searchAttribute.Related == KindProperty.ENUM ? GetDescription(s.PropertyType) : new Dictionary<int, string>(),
                    IndexClass = index,
                    Info = ResourceExtension.ResourceModel(searchAttribute.Related, searchAttribute.Index),
                    IsRequired = searchAttributeInput?.required!=null,
                    IsUnique = searchAttributeInput?.unique != null,
                    AutoNumeric = searchAttribute.GetType() == typeof(AutoNumericSearchAttribute),
                    Visible = searchAttribute.Visible,
                    HasInput = searchAttributeInput != null
                    
                    
                };
            }).ToArray();


            //si existe alguna propiead que esté en el input y no esté en la entidad.
            if (elemTypeInputProps.Any(s=>!props.Any(a=>a.Name.Equals(s.info.Name))))
            {
                var extra = elemTypeInputProps.Where(s => !props.Any(a => a.Name.Equals(s.info.Name)));
                var list = props.ToList();
                foreach (var item in extra)
                {
                    list.Add(new PropertySearchInfo
                    {
                        IsEnumerable = IsEnumerableProperty(item.info),
                        Name = item.info.Name,
                        Index = item.search.Index,
                        Related = item.search.Related,
                        Enums = item.search.Related == KindProperty.ENUM ? GetDescription(item.info.PropertyType) : new Dictionary<int, string>(),
                        IndexClass = index,
                        Info = ResourceExtension.ResourceModel(item.search.Related, item.search.Index),
                        IsRequired = item?.required != null,
                        IsUnique = item?.unique != null,
                       
                    });
                }
                props = list.ToArray();

            }
            return props;
        }

        public static PropertySearchInfo[] GetPropertySearchInfo(Type type) {
            var classAtribute = GetAttributes<ReferenceSearchHeaderAttribute>(type);
            if (classAtribute == null || !classAtribute.Any())
                return Array.Empty<PropertySearchInfo>();
            return classAtribute.SelectMany(s => GetPropertyByIndex(type, s.Index)).ToArray();
        }

    }

}