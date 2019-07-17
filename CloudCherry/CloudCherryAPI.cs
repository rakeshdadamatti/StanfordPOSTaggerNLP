using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;

namespace CloudCherry
{
    public class CloudCherryAPI
    {
        private HttpClient client = Client.Instance;

        public async Task<string> GetToken(string baseUrl, string username, string password)
        {
            string token = string.Empty;
            string endPoint = "api/LoginToken";
            string grantType = "password";
            Dictionary<string, string> formData = new Dictionary<string, string>
                {
                    {"grant_type", grantType},
                    {"username", username},
                    {"password", password},
                };
            
            try
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("cache-control", "no-cache");
                HttpRequestMessage req = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(baseUrl + endPoint)
                };
                req.Content = new FormUrlEncodedContent(formData);
                req.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                HttpResponseMessage response = await client.SendAsync(req).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    token = JsonConvert.DeserializeObject<dynamic>(jsonContent).access_token;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return token;
        }


        public async Task<dynamic> GetActiveQuestions(string baseUrl, string token)
        {
            string endPoint = "api/Questions/Active";
            dynamic jsonData = null;
            try
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("cache-control", "no-cache");
                HttpRequestMessage req = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(baseUrl + endPoint),
                };
                req.Headers.Add("Authorization", "Bearer " + token);
                HttpResponseMessage response = await client.SendAsync(req).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    jsonData = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return jsonData;
        }

        public async Task<dynamic> GetAnswers(string baseUrl, string token, int pageNumber, int noOfRows, Dictionary<string,string> payload)
        {
            string endPoint = "api/Answers/Page/" + pageNumber + "/" + noOfRows;
            dynamic jsonData = null;
            try
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("cache-control", "no-cache");
                string bodyContent = JsonConvert.SerializeObject(payload);
                HttpRequestMessage req = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(baseUrl + endPoint),
                    Content = new StringContent(bodyContent, Encoding.UTF8, "application/json")
                };
                req.Headers.Add("Authorization", "Bearer " + token);
                HttpResponseMessage response = await client.SendAsync(req).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    jsonData = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return jsonData;
        }

    }
}
