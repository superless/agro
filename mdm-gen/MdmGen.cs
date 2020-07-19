using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using trifenix.connect.mdm.entity_model;
using trifenix.connect.mdm.enums;
using trifenix.connect.mdm.ts_model;
using trifenix.connect.ts_model.enums;
using TypeGen.Core.Converters;
using TypeGen.Core.Generator;
using TypeGen.Core.SpecGeneration;


namespace mdm_gen
{
    public static class MdmGen
    {

        /// <summary>
        /// Borra recursivamente una carpeta
        /// </summary>
        /// <param name="baseDir">carpeta a borrar</param>
        public static void RecursiveDelete(DirectoryInfo baseDir)
        {
            if (!baseDir.Exists)
                return;

            foreach (var dir in baseDir.EnumerateDirectories())
            {
                RecursiveDelete(dir);
            }


            var files = baseDir.GetFiles();


            foreach (var file in files)
            {
                file.IsReadOnly = false;
                file.Delete();
            }
            baseDir.Delete();
        }


        /// <summary>
        /// Genera las clases en typescript desde el repositorio que se le asigne
        /// para luego actualizar la rama develop del componente
        /// </summary>
        /// <param name="gitAddress">repositorio git donde se actualizará</param>
        /// <param name="email"></param>
        /// <param name="username"></param>
        public static void GenerateMdm(string gitAddress, string email, string username) {

            var folder = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), $"mdm-gen-{Guid.NewGuid()}")).FullName;

            Colorful.Console.WriteLine($"Clonando repositorio", Color.OrangeRed);
            


            using (var repo = new Repository(Repository.Clone(gitAddress, folder, new CloneOptions {  BranchName = "develop" })))
            {
                
                Colorful.Console.WriteLine($"Repositorio clonado", Color.OrangeRed);

                var srcFolder = Path.Combine(folder, "src");

                Colorful.Console.WriteLine($"Eliminando archivos generados anteriormente", Color.OrangeRed);

                RecursiveDelete(new DirectoryInfo(srcFolder));

                Commands.Stage(repo, "*");

                repo.Commit("delete model mdm-gen", new Signature(username, email, DateTimeOffset.Now), new Signature(username, email, DateTimeOffset.Now));

                GenerateTsModel(srcFolder);

                Commands.Stage(repo, "*");

                Colorful.Console.WriteLine($"Commit con archivos generados", Color.OrangeRed);

                var status = repo.RetrieveStatus();


                Commands.Checkout(repo, "develop");
                
                repo.Commit("update model from mdm-gen", new Signature(username, email, DateTimeOffset.Now), new Signature(username, email, DateTimeOffset.Now));


                repo.Network.Push(repo.Network.Remotes["origin"], @$"refs/heads/develop", new PushOptions { });
            }


        }

        

        private static void GenerateTsModel(string directory) {
            var options = new GeneratorOptions
            {
                BaseOutputDirectory = directory,
                CreateIndexFile = true,
                PropertyNameConverters = new MemberNameConverterCollection(new IMemberNameConverter[] { new JsonMemberNameConverter(), new PascalCaseToCamelCaseConverter() }),
                FileNameConverters = new TypeNameConverterCollection(new ITypeNameConverter[] {  } ),
                SingleQuotes = true
            };

            var gen = new Generator(options);

            gen.FileContentGenerated += Gen_FileContentGenerated;


            gen.Generate(new List<GenerationSpec>() { new ModelSpec() });
            
        }



        private static void Gen_FileContentGenerated(object sender, FileContentGeneratedArgs e)
        {
            Colorful.Console.WriteLine($"Generando archivo {new FileInfo(e.FilePath).Name}", Color.Firebrick);
        }
    }

    


    public class ModelSpec : GenerationSpec {

        public override void OnBeforeGeneration(OnBeforeGenerationArgs args)
        {
            
            AddInterface<PropertyMetadadataEnum>("data/");
            AddInterface<EntitySearchDisplayInfo>("data/");
            AddInterface<PropertyMetadata>("data/");
            AddInterface<ModelMetaData>("data/");
            AddInterface<EntityMetadata>("data/");
            AddInterface<EntitySearch>("model/main");
            AddInterface<GeoPointTs>("model/main").CustomBase("");
            AddInterface(typeof(IProperty<>), "model/main");
            AddInterface<GeographyProperty>("model/main").CustomBase("");
            
            AddInterface<DblProperty>("model/main");
            AddInterface<DtProperty>("model/main");
            AddInterface<EnumProperty>("model/main");
            AddInterface<StrProperty>("model/main");
            AddInterface<Num32Property>("model/main");
            AddInterface<Num64Property>("model/main");
            AddInterface<BoolProperty>("model/main");
            AddInterface(typeof(PropertyBaseFaceTable<>),"model/main");
            AddInterface<IPropertyFaceTable>("model/main");

            AddInterface<RelatedId>("model/main");
            AddInterface<StrProperty>("model/main");
            AddEnum<PhisicalDevice>("model/enums");
            AddEnum<FilterType>("model/enums");

            AddInterface<FilterGlobalEntityInput>("model/filters").Member(nameof(FilterGlobalEntityInput.FilterChilds)).Optional();

            AddInterface<GroupInput>("model/containers")
                .Member(nameof(GroupInput.ColumnProportion)).Optional()                
                .Member(nameof(GroupInput.OrderIndex)).Optional()
                .Member(nameof(GroupInput.Device)).Optional();

            AddInterface<GroupMenu>("model/containers");





            AddInterface<FilterModel>("model/filters")
                .Member(nameof(FilterModel.BoolFilters)).Optional()
                .Member(nameof(FilterModel.DateFilters)).Optional()
                .Member(nameof(FilterModel.DoubleFilters)).Optional()
                .Member(nameof(FilterModel.EnumFilter)).Optional()
                .Member(nameof(FilterModel.FilterEntity)).Optional()
                .Member(nameof(FilterModel.FilterStr)).Optional()
                .Member(nameof(FilterModel.LongFilter)).Optional()
                .Member(nameof(FilterModel.NumFilter)).Optional()
                ;


            AddInterface(typeof(FilterBase<>), "model/filters");

            AddInterface<OrderItem>("model/containers");


            AddInterface<Facet>("model/containers");


            AddInterface(typeof(CollectionResult), "model/containers").Member(nameof(CollectionResult.Entities))
                .Member(nameof(CollectionResult.IndexPropNames)).Optional()
                .Member(nameof(CollectionResult.Filter)).Optional()
                .Member(nameof(CollectionResult.Facets)).Optional();

        }

        public override void OnBeforeBarrelGeneration(OnBeforeBarrelGenerationArgs args)
        {
            AddBarrel("model/main", BarrelScope.Files);
            AddBarrel("model/enums", BarrelScope.Files);
            AddBarrel("model/filters", BarrelScope.Files | BarrelScope.Directories);
            AddBarrel("model/containers", BarrelScope.Files | BarrelScope.Directories);
            AddBarrel("data", BarrelScope.Files | BarrelScope.Directories);
            AddBarrel("model", BarrelScope.Directories);
            

            AddBarrel(".", BarrelScope.Directories);
        }
    }

    

    /// <summary>
    /// toma el nombre que se asignó en un atributo jsonProperty y se lo asigna, si existe. de lo contrario
    /// tomará el nombre de la propiedad.
    /// </summary>
    public class JsonMemberNameConverter : IMemberNameConverter
    {
        //TypeNameConverterCollection

      

        public string Convert(string name, MemberInfo memberInfo)
        {
            var attribute = memberInfo.GetCustomAttribute<JsonPropertyAttribute>();
            
            var jsonname = attribute != null ? attribute.PropertyName : name;

            if (memberInfo.GetType().IsInterface)
            {
                return jsonname.Substring(1);
            }

            return jsonname;
        }
    }

    public class TypeMemberConverter : ITypeNameConverter
    {
        public string Convert(string name, Type type)
        {
            if (type.IsInterface)
            {
                return name.Substring(1);
            }
            return name;
        }
    }



    public class EntitySearch 
    {
        public string id { get; set; }
        public DateTime created { get; set; }
        public int index { get; set; }
        public BoolProperty[] bl { get; set; }
        public DblProperty[] dbl { get; set; }
        public DtProperty[] dt { get; set; }
        public EnumProperty[] enm { get; set; }
        public Num32Property[] num32 { get; set; }
        public Num64Property[] num64 { get; set; }
        public RelatedId[] rel { get; set; }
        public StrProperty[] str { get; set; }
        public StrProperty[] sug { get; set; }
        public GeographyProperty geo { get; set; }
    }
}
