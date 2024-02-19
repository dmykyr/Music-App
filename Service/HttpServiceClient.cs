using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MusicApp.Service
{
    public class HttpServiceClient : IDisposable
    {
        private static HttpClient _serviceClient = new()
        {
            BaseAddress = new Uri("http://127.0.0.1:5164/api/"),
            DefaultRequestHeaders =
            {
                Accept = { new MediaTypeWithQualityHeaderValue("application/json") }
            }
        };

        public async Task<TResponse> GetAsync<TResponse>(string endpoint)
        {
            HttpResponseMessage response = await _serviceClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<TResponse>();
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _serviceClient.PostAsync(endpoint, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<TResponse>();
        }

        public async Task<TResponse> PatchAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _serviceClient.PatchAsync(endpoint, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<TResponse>();
        }

        public async Task<string> DeleteAsync(string endpoint)
        {
            HttpResponseMessage response = await _serviceClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public void Dispose()
        {
            _serviceClient.Dispose();
        }
    }
}
