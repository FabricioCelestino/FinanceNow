using System.Net.Http.Json;
using System.Text.Json;
using FinanceNow.Blazor.Client.Modelos;
using FinanceNow.Blazor.Client.Request;

namespace FinanceNow.Blazor.Client.Services
{
    public class ApiService(HttpClient httpClient)
    {
        public async Task<T> GetAsync<T>(string url)
        {
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        }

        public async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            return await httpClient.DeleteAsync(url);
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string url, T data)
        {
            return await httpClient.PostAsJsonAsync<T>(url, data);
            
        }

        public async Task<HttpResponseMessage> PutAsync<T>(string url, T data)
        {
            return await httpClient.PutAsJsonAsync(url, data);
           
        }
    }
}
