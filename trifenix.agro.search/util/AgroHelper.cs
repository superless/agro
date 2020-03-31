using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using trifenix.agro.attr;
using trifenix.agro.enums.searchModel;

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
                if (IsEnumerable(value)) {
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

        public static bool IsEnumerable(this object element) => !element.GetType().Equals(typeof(string)) && element is IEnumerable<object>;

        public static bool IsEnumerableProperty(PropertyInfo propertyInfo) {
            var propertyType = propertyInfo.PropertyType;
            if (propertyType.Equals(typeof(string)))
                return false;
            return typeof(IEnumerable).IsAssignableFrom(propertyType);
        }

    }

    public class PropertySearchInfo {
        public SearchAttribute SearchAttribute { get; set; }
        public string Name { get; set; }
        public int IndexClass { get; set; }
        public Dictionary<int, string> Enums { get; set; }
        public bool IsEnumerable { get; set; }
    }

}