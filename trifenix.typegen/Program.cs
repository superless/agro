﻿using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Reflection;
using trifenix.agro.search.model.temp;
using trifenix.typegen.data;
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




            var jsonDataElements = JsonData.GetJsonData();



            

            var options = new GeneratorOptions
            {   
                BaseOutputDirectory = @"D:\aresa\developments\components\trifenix.search.model\src\",
                FileNameConverters = new TypeNameConverterCollection(new List<ITypeNameConverter>() { new CustomTypeConverter() }),
                TypeNameConverters = new TypeNameConverterCollection(new List<ITypeNameConverter>() { new CustomTypeConverter() }),
                PropertyNameConverters = new MemberNameConverterCollection(new IMemberNameConverter[] { new JsonMemberNameConverter() , new PascalCaseToCamelCaseConverter() })
                
            };

            string json = JsonConvert.SerializeObject(jsonDataElements, new JsonSerializerSettings {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            var gen = new Generator(options);
            gen.Generate(new List<GenerationSpec>() { new ModelSpec() });



            System.IO.File.WriteAllText($@"{options.BaseOutputDirectory}\data\data.ts", $"import {{IModelByIndex }} from \"./IModelByIndex\"; \nexport const data:IModelByIndex = {json} as IModelByIndex");

            Console.WriteLine("Codigo Generado en TypeScript");
        }
    }
}
