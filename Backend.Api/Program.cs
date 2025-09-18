using Backend.Services;


var builder = WebApplication.CreateBuilder(args);


//builder.Services.AddHttpClient<ApiService>();
builder.Services.AddHttpClient<SpotifyService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

app.UseCors();

app.MapGet("/artists/{id}", async (string id, SpotifyService spotify) =>
{
    var token = await spotify.GetAccessTokenAsync();
    var data = await spotify.GetArtistAsync(id, token);
    return Results.Content(data, "application/json");
});

app.MapGet("/artists/search", async ([FromQuery] string name, SpotifyService spotify) =>
{
  if (string.IsNullOrEmpty(name))
        return Results.BadRequest(new { error = "name query param required" });
  
  var token = await spotify.GetAccessTokenAsync();
  var data = await spotify.SearchArtistAsync(name, token);

  return Results.Content(data, "application/json");
});


app.Run("http://localhost:5000");
