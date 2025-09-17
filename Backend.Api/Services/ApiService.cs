using System.Net.Http;
using System.Threading.Tasks;


// ME FALTA CREAR TOKEN EN SPOTIFY DEVS

namespace Backend.Api.Services
{
  public class ApiService
  {
    private readonly HttpClient _httpClient;

    public ApiService
    {
      _httpClient = httpClient;
    }

    public async Task<string> GetExternalDataAsync()
    {
      var response = await _httpClient.GetAsync('');

      response.EnsureSuccessStatusCode();

      return await response.Content.ReadAsStringAsync();
    }
  }
}
