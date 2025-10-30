using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Fomo.Infrastructure.ExternalServices
{
    public class ExternalApiHelper : IExternalApiHelper
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _apiSettings;

        public ExternalApiHelper (IOptions<ApiSettings> options, HttpClient httpClient) 
        {
            _httpClient = httpClient;
            _apiSettings = options.Value;
            _httpClient.BaseAddress = new Uri(_apiSettings.BaseUrl);
        }

        public async Task<T> GetAsync<T> (string endpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{endpoint}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(json);
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception: {ex.Message}");
            }
        }
    }
}
