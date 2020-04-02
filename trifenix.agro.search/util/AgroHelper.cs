

using res.core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using trifenix.agro.attr;
using trifenix.agro.enums.searchModel;
using trifenix.agro.search.model.reflection;
using trifenix.agro.util;


namespace trifenix.agro.search.operations.util {
    public static class AgroHelper {

        


        public static Dictionary<SearchAttribute, object> GetPropertiesByAttribute(this object Obj) =>
            Obj.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(SearchAttribute), true)).ToDictionary(prop => (SearchAttribute)prop.GetCustomAttributes(typeof(SearchAttribute), true).FirstOrDefault(), prop => prop.GetValue(Obj));

        public static Dictionary<SearchAttribute, object> GetPropertiesByAttributeWithValue(this object Obj) =>
            GetPropertiesByAttribute(Obj).Where(s => ReflectionExtension.HasValue(s.Value)).ToDictionary(s => s.Key, s => s.Value);

        public static object[] GetPropertiesWithoutAttribute(this object Obj)  =>
            Obj.GetType().GetProperties().Where(prop => !Attribute.IsDefined(prop, typeof(SearchAttribute), true)).Select(prop => prop.GetValue(Obj)).ToArray();

        public static object[] GetPropertiesWithoutAttributeWithValues(this object Obj) =>
             GetPropertiesWithoutAttribute(Obj).Where(s => ReflectionExtension.HasValue(s)).ToArray();

        

        public static Type GetEntityType(EntityRelated index, Type typeInNameSpace, string nameSpace)
        {
            var assembly = Assembly.GetAssembly(typeInNameSpace);
            var modelTypes = assembly.GetLoadableTypes().Where(type => type.FullName.StartsWith(nameSpace) && Attribute.IsDefined(type, typeof(ReferenceSearchAttribute)));
            var entityType = modelTypes.Where(type => type.GetTypeInfo().GetCustomAttributes<ReferenceSearchAttribute>().Any(s=>s.Index == (int)index)).FirstOrDefault();
            return entityType;
        }









    }




}