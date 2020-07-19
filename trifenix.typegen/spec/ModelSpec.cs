using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.mdm.entity_model;
using trifenix.connect.mdm.enums;
using trifenix.connect.mdm.ts_model;
using trifenix.connect.ts_model.enums;
using TypeGen.Core.Converters;
using TypeGen.Core.Extensions;
using TypeGen.Core.SpecGeneration;
using TypeGen.Core.TypeAnnotations;

namespace trifenix.typegen.spec
{



    public class EntitySearch : IEntitySearch<GeoPointTs>
    {
        public string id { get; set; }
        public DateTime created { get; set; }
        public int index { get; set; }
        public IBoolProperty[] bl { get; set; }
        public IDblProperty[] dbl { get; set; }
        public IDtProperty[] dt { get; set; }
        public IEnumProperty[] enm { get; set; }
        public INum32Property[] num32 { get; set; }
        public INum64Property[] num64 { get; set; }
        public IRelatedId[] rel { get; set; }
        public IStrProperty[] str { get; set; }
        public IStrProperty[] sug { get; set; }
        public IProperty<GeoPointTs>[] geo { get; set; }
    }

    /// Modelo de generación de un modelo de typescript.
    /// </summary>
    public class ModelSpec : GenerationSpec {
        public override void OnBeforeGeneration(OnBeforeGenerationArgs args) {
            AddInterface<PropertyMetadadataEnum>("data/");
            AddInterface<EntitySearchDisplayInfo>("data/");
            AddInterface<PropertyMetadata>("data/");
            AddInterface<ModelMetaData>("data/");
            AddInterface<EntityMetadata>("data/");            
            AddInterface<EntitySearch>("model/main");
            AddInterface<GeoPointTs>("model/main");
            AddInterface(typeof(IProperty<>), "model/main");
            AddInterface<GeographyProperty>("model/main");            
            AddInterface<EntitySearch>("model/main");
            AddInterface<IDblProperty>("model/main");
            AddInterface<IDtProperty>("model/main");
            AddInterface<IEnumProperty>("model/main");
            AddInterface<IStrProperty>("model/main");
            AddInterface<INum32Property>("model/main");
            AddInterface<INum64Property>("model/main");            
            AddInterface<IBoolProperty>("model/main");
            AddInterface(typeof(IPropertyBaseFaceTable<>), "model/main");
            AddInterface<IRelatedId>("model/main");
            AddInterface<IStrProperty>("model/main");
            AddEnum<PhisicalDevice>("model/enums");
            AddEnum<FilterType>("model/enums");



            var enumAssembly = Assembly.GetAssembly(typeof(EntityRelated));

            IEnumerable<Type> enumTypes = enumAssembly.GetLoadableTypes();

            var enumSearch = enumTypes.Where(x => x.FullName.StartsWith("trifenix.agro.enums.yo_fenix"));





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

            AddInterface<OrderItem>("model/ts");


            AddInterface<Facet>("model/ts");


            AddInterface(typeof(CollectionResult), "model/ts").Member(nameof(CollectionResult.Entities)).Type("EntitySearch[]", "./../main/entity-search")
                .Member(nameof(CollectionResult.IndexPropNames)).Optional()
                .Member(nameof(CollectionResult.Filter)).Optional()
                .Member(nameof(CollectionResult.Facets)).Optional();

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

    /// <summary>
    /// toma el nombre que se asignó en un atributo jsonProperty y se lo asigna, si existe. de lo contrario
    /// tomará el nombre de la propiedad.
    /// </summary>
    public class JsonMemberNameConverter : IMemberNameConverter {
        public string Convert(string name, MemberInfo memberInfo) {
            var attribute = memberInfo.GetCustomAttribute<JsonPropertyAttribute>();
            return attribute != null ? attribute.PropertyName : name;
        }
    }

   

}