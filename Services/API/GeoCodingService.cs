using System.Text.Json;

namespace SportCourtManagement.Services.API
{
    public class GeoCodingService
    {
        private readonly HttpClient _httpClient;

        public GeoCodingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(double lat, double lon)?> GetCoordinatesAsync(string address)
        {
            var url = $"https://nominatim.openstreetmap.org/search?format=json&q={Uri.EscapeDataString(address)}";

            _httpClient.DefaultRequestHeaders.Add("User-Agent", "MyAspNetApp"); 
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            var results = JsonSerializer.Deserialize<List<NominatimResult>>(json);

            if (results == null || results.Count == 0)
                return null;

            double lat = double.Parse(results[0].lat);
            double lon = double.Parse(results[0].lon);
            return (lat, lon);
        }

        private class NominatimResult
        {
            public string lat { get; set; }
            public string lon { get; set; }
        }
    }

}
