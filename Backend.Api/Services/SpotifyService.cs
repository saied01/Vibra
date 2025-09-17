using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Backend.Services
{
    public class SpotifyService
    {
        private readonly HttpClient _httpClient;
        private readonly string _clientId = "9b6d90e3ce66472e9b0f350190bdbbe2";
        private readonly string _clientSecret = "d269bc221c4f4d2cb5ec6047f0d81fe5";

        public SpotifyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Obtener access token
        public async Task<string> GetAccessTokenAsync()
        {
            var auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_clientId}:{_clientSecret}"));

            var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", auth);
            request.Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("grant_type", "client_credentials")
            });

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("access_token").GetString();
        }

        // Obtener info de artista por ID
        public async Task<string> GetArtistAsync(string artistId, string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.GetAsync($"https://api.spotify.com/v1/artists/{artistId}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> SearchArtistAsync(string name, string accessToken)
        {
          _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);

          var request = $"https://api.spotify.com/v1/search?q={Uri.EscapeDataString(name)}&type=artist&limit=5";

          var response = await _httpClient.GetAsync(request);
          response.EnsureSuccessStatusCode();

          return await response.Content.ReadAsStringAsync();
        }
    }
}
