using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using trifenix.agro.attr;
using trifenix.agro.search.model.ts;

namespace trifenix.agro.search.operations.util
{
    public static class AgroHelper
    {
        public static string GetDescription(this Enum GenericEnum) //Hint: Change the method signature and input paramter to use the type parameter T
        {
            Type genericEnumType = GenericEnum.GetType();
            MemberInfo[] memberInfo = genericEnumType.GetMember(GenericEnum.ToString());
            if ((memberInfo != null && memberInfo.Length > 0))
            {
                var _Attribs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if ((_Attribs != null && _Attribs.Count() > 0))
                {
                    return ((System.ComponentModel.DescriptionAttribute)_Attribs.ElementAt(0)).Description;
                }
            }
            return GenericEnum.ToString();
        }

        public static Dictionary<int, string> GetDescription(Type type)
        {

            var values = Enum.GetValues(type);

            var enumElements = Enum.GetValues(type).Cast<Enum>();

            var dict = enumElements.ToDictionary(s => (int)(object)s, g => g.GetDescription());

            return dict;

        }
        public class PropertySearchInfo {
            public string Name { get; set; }
            public SearchAttribute SearchAttribute { get; set; }

            public bool Array { get; set; }

            public int IndexClass { get; set; }



            public Dictionary<int, string> Enums { get; set; }

        }

        public static PropertySearchInfo[] GetPropertySearchInfo(Type type) {


            var classAtribute = type.GetCustomAttribute<ReferenceSearchAttribute>();

            if (classAtribute == null) return Array.Empty<PropertySearchInfo>();

            var indexClass = classAtribute.Index;

            var props = type.GetProperties();

            var searchAttributes = props.Where(prop => Attribute.IsDefined(prop, typeof(SearchAttribute), true));


            return searchAttributes.Select(s => {

                var se = (SearchAttribute)s.GetCustomAttributes(typeof(SearchAttribute), true).FirstOrDefault();


                return new PropertySearchInfo
                {
                    Array = IsEnumerableProperty(s),
                    Name = s.Name,
                    SearchAttribute = se,
                    Enums = se.Related==Related.ENUM?GetDescription(s.PropertyType):new Dictionary<int, string>(),
                    IndexClass = indexClass


                    
                };
            }).ToArray();
            
        }

        private static bool GetObjectWithValue(this object value)
        {
            if (isArray(value))
            {
                if (!((IEnumerable<object>)value).Any()) return false;

            }
            else
            {
                if (!(value is object) || value == null)
                {
                    return false;
                }
            }
            return true;
        }

        public static Dictionary<SearchAttribute, object> GetPropertiesByAttributeWithValue(this object Obj) {
            var props = GetPropertiesByAttribute(Obj);

            return props.Where(s => GetObjectWithValue(s.Value)).ToDictionary(s => s.Key, s => s.Value);
        }

        public static bool isArray(this object element)
        {
            return element is IEnumerable<object>;
        }

        public static Dictionary<SearchAttribute, object> GetPropertiesByAttribute(this object Obj)  => Obj.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(SearchAttribute), true)).ToDictionary(prop => (SearchAttribute)prop.GetCustomAttributes(typeof(SearchAttribute), true).FirstOrDefault(), prop => prop.GetValue(Obj));




        public static object[] GetPropertiesWithoutAttribute(this object Obj)  =>
            Obj.GetType().GetProperties(BindingFlags.Public).Where(prop => !Attribute.IsDefined(prop, typeof(SearchAttribute), true)).
            Select(prop => prop.GetValue(Obj)).ToArray();


        public static object[] GetPropertiesWithoutAttributeWithValues(this object Obj) =>
             GetPropertiesWithoutAttribute(Obj).Where(s => GetObjectWithValue(s)).ToArray();






        public static bool IsEnumerableProperty(PropertyInfo propertyInfo)
        {
            var propType = propertyInfo.PropertyType;

            var ienumerableInterfaces = propType.GetInterfaces()
                    .Where(x => x.IsGenericType && x.GetGenericTypeDefinition() ==
                                typeof(IEnumerable<>)).ToList();




            return ienumerableInterfaces.All(x =>
                        !x.GenericTypeArguments[0].IsValueType);


        }
    }
}
