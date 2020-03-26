using System;
using System.Collections.Generic;
using System.Reflection;
using trifenix.agro.search.model.temp;
using trifenix.typegen.spec;
using TypeGen.Core.Converters;
using TypeGen.Core.Generator;
using TypeGen.Core.SpecGeneration;

namespace trifenix.typegen
{
    class Program
    {
        static void Main(string[] args)
        {

            var options = new GeneratorOptions
            {
                BaseOutputDirectory = @"d:\test\ts",
                TypeNameConverters = new TypeNameConverterCollection(new List<ITypeNameConverter>() { new CustomTypeConverter() }),
                PropertyNameConverters = new MemberNameConverterCollection(new IMemberNameConverter[] { new JsonMemberNameConverter() })


                
            };
            var gen = new Generator(options);
            

            //var assembly = Assembly.GetAssembly(typeof(EntitySearchV2));

            gen.Generate(new List<GenerationSpec>() { new ModelSpec() });


            
            Console.WriteLine("Hello World!");
        }
    }
}
