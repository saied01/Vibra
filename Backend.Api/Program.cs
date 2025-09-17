using Backend.Api.Services;
using Backend.Api.Controllers;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddHttpClient<ApiService>();
builder.Services.AddHttpClient<ApiService>();

var app = builder.Build();


app.MapGet("/artists/{id}", async (string id, SpotifyService spotify) =>
{
    var token = await spotify.GetAccessTokenAsync();
    var data = await spotify.GetArtistAsync(id, token);
    return Results.Content(data, "application/json");
});

app.Run("http://localhost:5000");
