using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

using trifenix.typegen.spec;
using TypeGen.Core.Converters;
using TypeGen.Core.Generator;
using TypeGen.Core.SpecGeneration;

namespace trifenix.typegen {
    class Program {
        static void Main(string[] args) {

            

            var options = new GeneratorOptions
            {
                BaseOutputDirectory = @"G:\ale-folder\fullgit\2\newcomponent\fenix-metadata\src",
                PropertyNameConverters = new MemberNameConverterCollection(new IMemberNameConverter[] { new JsonMemberNameConverter(), new PascalCaseToCamelCaseConverter() }),
                SingleQuotes = true
            };

           

            var gen = new Generator(options);
            gen.Generate(new List<GenerationSpec>() { new ModelSpec() });

            

            Console.WriteLine("Codigo Generado en TypeScript");
        }
    }
}
