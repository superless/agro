using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using trifenix.agro.search.model.temp;
using TypeGen.Core.Converters;
using TypeGen.Core.SpecGeneration;

namespace trifenix.typegen.spec
{
    public class ModelSpec : GenerationSpec
    {
        public override void OnBeforeGeneration(OnBeforeGenerationArgs args)
        {
            AddInterface<EntitySearch>("base/model");
            AddInterface<GeoPointTs>("base/model/geo");
            AddInterface<GeoProperty>("base/model/geo").Member(nameof(GeoProperty.Value)).Type("IGeoPointTs","./geo-point-ts");

        }
        

        
    }

    public class JsonMemberNameConverter : IMemberNameConverter
    {
        public string Convert(string name, MemberInfo memberInfo)
        {
            var attribute = memberInfo.GetCustomAttribute<JsonPropertyAttribute>();
            return attribute != null ? attribute.PropertyName : name;
        }

        
    }

    public class CustomTypeConverter : ITypeNameConverter
    {
        public string Convert(string name, Type type)
        {
            if (type.IsClass) {
                return $"I{name}";
            }
            return name;
        }
    }
}
