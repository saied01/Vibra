using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Backend.Api.Services;

namespace Backend.Api.Controllers
{
    public static class DataController
    {
        public static void MapRoutes(WebApplication app)
        {
            app.MapGet("/data", async (ApiService apiService) =>
            {
                var result = await apiService.GetExternalDataAsync();
                return Results.Content(result, "application/json");
            });
        }
    }
}
