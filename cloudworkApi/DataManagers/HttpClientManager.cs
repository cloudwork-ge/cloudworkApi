using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace cloudworkApi.DataManagers
{
    public class HttpClientManager
    {
        public HttpClient http = new HttpClient();
        public async Task<T> GetAndDeserialize<T>(string url, string responseRootProperty = "")
        {
            HttpResponseMessage res = await http.GetAsync(url);
            var responseStr = "";
            if (res.IsSuccessStatusCode)
            responseStr = res.Content.ReadAsStringAsync().Result;

            var strToConvert = responseStr;
            JsonDocument responseJson = null;
            if (!string.IsNullOrWhiteSpace(responseRootProperty))
            {
                responseJson = JsonDocument.Parse(responseStr);

                string[] split = null;
                if (responseRootProperty.Contains("=>"))
                {
                    split = responseRootProperty.Split("=>");
                    foreach(var prop in split)
                    {
                        strToConvert = Convert.ToString(JsonDocument.Parse(strToConvert).RootElement.GetProperty(prop));
                    }
                }
                else
                    strToConvert = Convert.ToString(responseJson.RootElement.GetProperty(responseRootProperty));

            }

            T serialized = default(T);
            if (!string.IsNullOrEmpty(strToConvert))
            serialized = JsonSerializer.Deserialize<T>(strToConvert);

            return serialized;
        }
    }
}
