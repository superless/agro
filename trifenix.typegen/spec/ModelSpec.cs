using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using trifenix.agro.enums.searchModel;
using trifenix.agro.search.model;
using trifenix.agro.search.model.reflection;
using trifenix.agro.search.model.ts;
using TypeGen.Core.Converters;
using TypeGen.Core.Extensions;
using TypeGen.Core.SpecGeneration;
using TypeGen.Core.TypeAnnotations;

namespace trifenix.typegen.spec {

    public class Data { }

    public class ModelSpec : GenerationSpec {
        public override void OnBeforeGeneration(OnBeforeGenerationArgs args) {
            AddInterface<EnumDictionary>("data/");
            AddInterface<EntitySearchDisplayInfo>("data/");
            AddInterface<DefaultDictionary>("data/");
            AddInterface<ModelMetaData>("data/");
            AddInterface<ModelDictionary>("data/");            
            AddInterface<EntitySearch>("model/main");
            AddInterface<GeoPointTs>("model/main");
            AddInterface(typeof(BaseProperty<>), "model/main");
            AddInterface<GeographyProperty>("model/main").Member(nameof(GeoProperty.Value));
            AddInterface<ReletadIdTs>("model/main").Member(nameof(RelatedId.Id)).Type(TsType.String);
            AddInterface<EntitySearch>("model/main")
                .Member(nameof(EntitySearch.GeoProperties)).Type("IGeographyProperty[]", "./IGeographyProperty")
                .Member(nameof(EntitySearch.RelatedIds)).Type("IReletadIdTs[]", "./IReletadIdTs");
            AddInterface<DblProperty>("model/main");
            AddInterface<DtProperty>("model/main");
            AddInterface<EnumProperty>("model/main");
            AddInterface<StrProperty>("model/main");
            AddInterface<Num32Property>("model/main");
            AddInterface<Num64Property>("model/main");
            AddInterface<SuggestProperty>("model/main");
            AddInterface<BoolProperty>("model/main");
            AddInterface(typeof(BaseFacetableProperty<>), "model/main").Member(nameof(StrProperty.Id)).Type(TsType.String);
            AddInterface<RelatedId>("model/main").Member(nameof(StrProperty.Id)).Type(TsType.String);
            AddClass<Data>("data");
            AddInterface<StrProperty>("model/main").Member(nameof(StrProperty.Id)).Type(TsType.String);
            var enumAssembly = Assembly.GetAssembly(typeof(EntityRelated));
            IEnumerable<Type> enumTypes = enumAssembly.GetLoadableTypes();

            //var enumModel = enumTypes.Where(x => x.FullName.StartsWith("trifenix.agro.enums.input") || x.FullName.StartsWith("trifenix.agro.enums.model"));

            //foreach (Type type in enumModel)
            //    AddEnum(type, "model/agro/enums");

            var enumSearch = enumTypes.Where(x => x.FullName.StartsWith("trifenix.agro.enums.searchModel"));

            foreach (Type type in enumSearch)
                AddEnum(type, "model/enums");

            //var assembly = Assembly.GetAssembly(typeof(ApplicationOrderInput));
            //IEnumerable<Type> types = assembly.GetLoadableTypes()
            //.Where(x => x.FullName.StartsWith("trifenix.agro.model.external.Input") && x.Name.Contains("Swagger"));
            //foreach (Type type in types) {
            //    AddInterface(type, "model/agro/main");
            //}
            //AddInterface(typeof(ExtGetContainer<>), "model/agro");
            //AddInterface(typeof(ExtPostContainer<>), "model/agro");

            AddEnum(typeof(TypeEntity), "model/ts/enum");
            AddInterface(typeof(SearchType), "model/ts")
                .Member(nameof(SearchType.MainEntityIndex)).Optional()
                .Member(nameof(SearchType.DataDependant)).Optional()
                .Member(nameof(SearchType.EntitySearchTypeIndex)).Optional()
                .Member(nameof(SearchType.CategoryIndex)).Optional()
                .Member(nameof(SearchType.PlaceHolder)).Optional()
                .Member(nameof(SearchType.MessageNotFound)).Optional()
                .Member(nameof(SearchType.PropertyCategoryIndex)).Optional()
                .Member(nameof(SearchType.PropertyIndex)).Optional();
            AddInterface(typeof(Result), "model/ts").Member(nameof(Result.Entities)).Type("IEntitySearch[]", "./../main/IEntitySearch");
        }

        public override void OnBeforeBarrelGeneration(OnBeforeBarrelGenerationArgs args) {
            AddBarrel("model/main", BarrelScope.Files);
            AddBarrel("model/enums", BarrelScope.Files);
            //AddBarrel("model/agro/enums", BarrelScope.Files);
            //AddBarrel("model/agro/main", BarrelScope.Files);
            // AddBarrel("model/agro/", BarrelScope.Files | BarrelScope.Directories);
            AddBarrel("model/ts/enum", BarrelScope.Files);
            AddBarrel("model/ts", BarrelScope.Files | BarrelScope.Directories);
            AddBarrel("data", BarrelScope.Files | BarrelScope.Directories);
            AddBarrel("model", BarrelScope.Directories);
            AddBarrel(".", BarrelScope.Directories);
        }
    }

    public class JsonMemberNameConverter : IMemberNameConverter {
        public string Convert(string name, MemberInfo memberInfo) {
            var attribute = memberInfo.GetCustomAttribute<JsonPropertyAttribute>();
            return attribute != null ? attribute.PropertyName : name;
        }
    }

    public class CustomTypeConverter : ITypeNameConverter {
        public string Convert(string name, Type type) {
            if (type.IsClass) {
                if (name.Contains("Swagger"))
                    name = name.Replace("Swagger", "");
                if (name.Equals("Data"))
                    return "data";
                return $"I{name}";
            }
            return name;
        }
    }

}