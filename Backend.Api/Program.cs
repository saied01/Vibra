using Backend.Services;


var builder = WebApplication.CreateBuilder(args);


//builder.Services.AddHttpClient<ApiService>();
builder.Services.AddHttpClient<SpotifyService>();

var app = builder.Build();


app.MapGet("/artists/{id}", async (string id, SpotifyService spotify) =>
{
    var token = await spotify.GetAccessTokenAsync();
    var data = await spotify.GetArtistAsync(id, token);
    return Results.Content(data, "application/json");
});

app.MapGet("/artists/search", async (string name, SpotifyService spotify) =>
{
  var token = await spotify.GetAccessTokenAsync();
  var data = await spotify.SearchArtistAsync(name, token);

  return Results.Content(data, "application/json");
});


app.Run("http://localhost:5000");
