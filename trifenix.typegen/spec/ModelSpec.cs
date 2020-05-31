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

           

            var enumSearch = enumTypes.Where(x => x.FullName.StartsWith("trifenix.agro.enums.searchModel"));

            foreach (Type type in enumSearch)
                AddEnum(type, "model/enums");



            AddInterface<FilterModel>("model/ts")
                .Member(nameof(FilterModel.BoolFilters)).Optional()
                .Member(nameof(FilterModel.DateFilters)).Optional()
                .Member(nameof(FilterModel.DoubleFilters)).Optional()
                .Member(nameof(FilterModel.EnumFilter)).Optional()
                .Member(nameof(FilterModel.FilterEntity)).Optional()
                .Member(nameof(FilterModel.FilterStr)).Optional()
                .Member(nameof(FilterModel.LongFilter)).Optional()
                .Member(nameof(FilterModel.NumFilter)).Optional()                
                ;
            AddInterface(typeof(FilterBase<>), "model/ts");
            AddInterface<Facet>("model/ts");
            AddInterface(typeof(Result), "model/ts").Member(nameof(Result.Entities)).Type("IEntitySearch[]", "./../main/IEntitySearch")
                .Member(nameof(Result.ByDesc)).Optional()
                .Member(nameof(Result.IndexSorted)).Optional()
                .Member(nameof(Result.EntityKindSort)).Optional()
                .Member(nameof(Result.Filter)).Optional()
                .Member(nameof(Result.Facets)).Optional()

                ;
        }

        public override void OnBeforeBarrelGeneration(OnBeforeBarrelGenerationArgs args) {
            AddBarrel("model/main", BarrelScope.Files);
            AddBarrel("model/enums", BarrelScope.Files);
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
                
                if (name.Equals("Data"))
                    return "data";
                return $"I{name}";
            }
            return name;
        }
    }

}