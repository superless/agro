using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace trifenix.typegen.data
{
    class Program
    {
        static void Main(string[] args)
        {
            // genera los datos
            var jsonDataElements = JsonData.GetJsonData();

            string json = JsonConvert.SerializeObject(jsonDataElements, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            // genera el json con datos
            System.IO.File.WriteAllText($@"\data\data.ts", $"import {{ ModelMetaData }} from \"./IModelMetaData\"; \nexport const data:IModelMetaData = {json} as IModelMetaData");

        }
    }
}
