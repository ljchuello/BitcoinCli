using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Core
{
    public class Core
    {
        private const string ApiServer = "http://127.0.0.1:8332";

        public static async Task<string> SendGetRequest()
        {
            HttpResponseMessage httpResponseMessage;
            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage(new HttpMethod("GET"), $"{ApiServer}"))
                {
                    httpRequestMessage.Headers.TryAddWithoutValidation("Authorization", $"Basic bGpjaHVlbGxvOjI1MDI5Mi4u");
                    httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                }
            }

            // IsNullOrEmpty
            string json = await httpResponseMessage.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(json))
            {
                throw new Exception("there has been some error, the API has responded empty.");
            }

            // No OK
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("{error.Message}");
            }

            return json;
        }


        public static async Task<string> SendPostRequestAsync(string content)
        {
            return await Task.Run(async () =>
            {
                HttpResponseMessage httpResponseMessage;
                using (HttpClient httpClient = new HttpClient())
                {
                    using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage(new HttpMethod("POST"), $"{ApiServer}"))
                    {
                        httpRequestMessage.Headers.TryAddWithoutValidation("Authorization", $"Basic bGpjaHVlbGxvOjI1MDI5Mi4u");
                        httpRequestMessage.Content = new StringContent(content);
                        httpRequestMessage.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                        httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                    }
                }

                // IsNullOrEmpty
                string json = await httpResponseMessage.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(json))
                {
                    throw new Exception("there has been some error, the API has responded empty.");
                }

                // No Created
                if (httpResponseMessage.StatusCode != HttpStatusCode.Created &&
                    httpResponseMessage.StatusCode != HttpStatusCode.OK)
                {
                    //JObject result = JObject.Parse(json);
                    //Error error = JsonConvert.DeserializeObject<Error>($"{result["error"]}") ?? new Error();
                    throw new Exception("{error.Message}");
                }

                return json;
            });
        }

        public async Task<string> SendPutRequest(string token, string url, string content)
        {
            HttpResponseMessage httpResponseMessage;
            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage(new HttpMethod("PUT"), $"{ApiServer}{url}"))
                {
                    httpRequestMessage.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");
                    httpRequestMessage.Content = new StringContent(content);
                    httpRequestMessage.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                    httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                }
            }

            // IsNullOrEmpty
            string json = await httpResponseMessage.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(json))
            {
                throw new Exception("there has been some error, the API has responded empty.");
            }

            // No Update
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                //JObject result = JObject.Parse(json);
                //Error error = JsonConvert.DeserializeObject<Error>($"{result["error"]}") ?? new Error();
                throw new Exception("{error.Message}");
            }

            return json;
        }

        public async Task SendDeleteRequest(string token, string url)
        {
            HttpResponseMessage httpResponseMessage;
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("DELETE"), $"{ApiServer}{url}"))
                {
                    request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");
                    httpResponseMessage = await httpClient.SendAsync(request);
                }
            }

            switch (httpResponseMessage.StatusCode)
            {
                case HttpStatusCode.NoContent:
                case HttpStatusCode.OK:
                    break;

                default:
                    string json = await httpResponseMessage.Content.ReadAsStringAsync();
                    //JObject result = JObject.Parse(json);
                    //Error error = JsonConvert.DeserializeObject<Error>($"{result["error"]}") ?? new Error();
                    throw new Exception("{error.Message}");
            }
        }
    }
}