using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using trifenix.agro.attr;
using trifenix.agro.db.model;
using trifenix.agro.enums.searchModel;
using trifenix.agro.util;
using static trifenix.agro.util.AttributesExtension;

namespace trifenix.agro.search.operations.util {
    public static class AgroHelper {

        public static string GetDescription(this Enum GenericEnum) {      
            Type genericEnumType = GenericEnum.GetType();
            MemberInfo[] memberInfo = genericEnumType.GetMember(GenericEnum.ToString());
            if (memberInfo != null && memberInfo.Any()) {
                var _Attribs = memberInfo.FirstOrDefault()?.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (_Attribs != null && _Attribs.Any())
                    return ((DescriptionAttribute)_Attribs.FirstOrDefault())?.Description;
            }
            return GenericEnum.ToString();
        }

        public static Dictionary<int, string> GetDescription(Type type) {
            var values = Enum.GetValues(type);
            var enumElements = Enum.GetValues(type).Cast<Enum>();
            var dict = enumElements.ToDictionary(s => (int)(object)s, g => g.GetDescription());
            return dict;
        }

        public static PropertySearchInfo[] GetPropertySearchInfo(Type type) {
            var classAtribute = type.GetCustomAttribute<ReferenceSearchAttribute>();
            if (classAtribute == null)
                return Array.Empty<PropertySearchInfo>();
            var indexClass = classAtribute.Index;
            var searchAttributesProps = type.GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(SearchAttribute), true));
            return searchAttributesProps.Select(s => {
                var SearchAttribute = (SearchAttribute)s.GetCustomAttributes(typeof(SearchAttribute), true).FirstOrDefault();
                return new PropertySearchInfo {
                    IsEnumerable = IsEnumerableProperty(s),
                    Name = s.Name,
                    SearchAttribute = SearchAttribute,
                    Enums = SearchAttribute.Related == Related.ENUM ? GetDescription(s.PropertyType) : new Dictionary<int, string>(),
                    IndexClass = indexClass
                };
            }).ToArray();
        }

        private static bool HasValue(this object value) {
            if (value == null)
                return false;
            else {
                if (value.IsEnumerable()) {
                    if (!((IEnumerable<object>)value).Any())
                        return false;
                }
                else {
                    if (!(value is object))
                        return false;
                }
            }
            return true;
        }

        public static Dictionary<SearchAttribute, object> GetPropertiesByAttribute(this object Obj) =>
            Obj.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(SearchAttribute), true)).ToDictionary(prop => (SearchAttribute)prop.GetCustomAttributes(typeof(SearchAttribute), true).FirstOrDefault(), prop => prop.GetValue(Obj));

        public static Dictionary<SearchAttribute, object> GetPropertiesByAttributeWithValue(this object Obj) =>
            GetPropertiesByAttribute(Obj).Where(s => HasValue(s.Value)).ToDictionary(s => s.Key, s => s.Value);

        public static object[] GetPropertiesWithoutAttribute(this object Obj)  =>
            Obj.GetType().GetProperties().Where(prop => !Attribute.IsDefined(prop, typeof(SearchAttribute), true)).Select(prop => prop.GetValue(Obj)).ToArray();

        public static object[] GetPropertiesWithoutAttributeWithValues(this object Obj) =>
             GetPropertiesWithoutAttribute(Obj).Where(s => HasValue(s)).ToArray();

        public static T CreateInstance<T>() => (T)Activator.CreateInstance(typeof(T));

        public static object CreateEntityInstance(Type genericParameterType) => typeof(AgroHelper).GetMethod("CreateInstance").MakeGenericMethod(genericParameterType).Invoke(null, null);

        public static Type GetEntityType(int index) {
            var assembly = Assembly.GetAssembly(typeof(Barrack));
            var modelTypes = assembly.GetLoadableTypes().Where(type => type.FullName.StartsWith("trifenix.agro.db.model") && Attribute.IsDefined(type,typeof(ReferenceSearchAttribute)));
            var entityType = modelTypes.Where(type => type.GetTypeInfo().GetCustomAttribute<ReferenceSearchAttribute>().Index == index).FirstOrDefault();
            return entityType;
        }

        public static object CastToGenericArray(Type genericParameterType, IEnumerable<object> list) => typeof(AgroHelper).GetMethod("CastToArray").MakeGenericMethod(genericParameterType).Invoke(null, new object[] { list });

        public static T[] CastToArray<T>(IEnumerable<object> list) => list.Select(element => (T)element).ToArray();

        public static object CastToGenericList(Type genericParameterType, IEnumerable<object> list) => typeof(AgroHelper).GetMethod("CastToList").MakeGenericMethod(genericParameterType).Invoke(null, new object[] { list });

        public static List<T> CastToList<T>(IEnumerable<object> list) => list.Select(element => (T)element).ToList();

    }

    public class PropertySearchInfo {
        public SearchAttribute SearchAttribute { get; set; }
        public string Name { get; set; }
        public int IndexClass { get; set; }
        public Dictionary<int, string> Enums { get; set; }
        public bool IsEnumerable { get; set; }
    }

}