using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using trifenix.connect.interfaces.upload.translate;

namespace trifenix.connect.translate
{

    public class GoogleTranslator : IGoogleTranslator {
        public string TranslateText(string textToTranslate) {
            HttpClient httpClient = new HttpClient();
            string langFrom = "en";
            string langTo = "es";
            string url = "https://translate.googleapis.com/translate_a/single?client=gtx&dt=t&sl=" + langFrom + "&tl=" + langTo + "&q=" + textToTranslate;
            string result = httpClient.GetStringAsync(url).Result;
            httpClient.Dispose();
            dynamic jsonData = JsonConvert.DeserializeObject(result);
            JArray jArray = jsonData[0];
            var enumerator = jArray.GetEnumerator();
            enumerator.MoveNext();
            string translation = (string)enumerator.Current[0];
            return translation;
        }

    }

}