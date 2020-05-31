using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Collections;
using System.ComponentModel;

namespace trifenix.agro.util {

    public static class ReflectionExtension {
        
        public static bool HasValue(this object value) {
            if (value == null)
                return false;
            else
                if (IsEnumerable(value)) {
                    if (!((IEnumerable<object>)value).Any())
                        return false;
                }
                else
                    if (!(value is object))
                        return false;
            return true;
        }

        public static bool IsEnumerable(this object element) => !element.GetType().Equals(typeof(string)) && element is IEnumerable<object>;

    }

    public static class AttributesExtension {

        public static T[] GetAttributes<T>(Type type) where T : Attribute => type.GetCustomAttributes<T>().ToArray();

        public static bool IsEnumerableProperty(PropertyInfo propertyInfo) {
            var propertyType = propertyInfo.PropertyType;
            if (propertyType.Equals(typeof(string)))
                return false;
            return typeof(IEnumerable).IsAssignableFrom(propertyType);
        }

        public static T GetAttribute<T>(this PropertyInfo propInfo) where T : Attribute => (T)propInfo.GetCustomAttributes(typeof(T), true).FirstOrDefault();

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
            var dict = enumElements.ToDictionary(s => (int)(object)s, g => GetDescription(g));
            return dict;
        }

    }

}