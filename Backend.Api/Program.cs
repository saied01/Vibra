
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

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

// GET /artists/{id}
app.MapGet("/artists/{id}", async (string id, SpotifyService spotify) =>
{
    var token = await spotify.GetAccessTokenAsync();
    var data = await spotify.GetArtistAsync(id, token);
    return Results.Content(data, "application/json");
});

// GET /artists/search?name=...
app.MapGet("/artists/search", async ([FromQuery] string name, SpotifyService spotify) =>
{
    if (string.IsNullOrEmpty(name))
        return Results.BadRequest(new { error = "name query param required" });

    var token = await spotify.GetAccessTokenAsync();
    var data = await spotify.SearchArtistAsync(name, token);

    return Results.Content(data, "application/json");
});

// GET /artists/recommend?name=...
app.MapGet("/artists/recommend", async ([FromQuery] string name, SpotifyService spotify) =>
{
    if (string.IsNullOrEmpty(name))
        return Results.BadRequest(new { error = "name query param required" });

    var token = await spotify.GetAccessTokenAsync();

    // Buscar artista por nombre
    var searchResultJson = await spotify.SearchArtistAsync(name, token);
    using var searchDoc = JsonDocument.Parse(searchResultJson);

    var items = searchDoc.RootElement
        .GetProperty("artists")
        .GetProperty("items");

    if (items.GetArrayLength() == 0)
        return Results.NotFound(new { error = "Artist not found." });

    var artistId = items[0].GetProperty("id").GetString();

    // Obtener artistas relacionados
    var relatedResultJson = await spotify.GetRelatedArtistsAsync(artistId!, token);

    return Results.Content(relatedResultJson, "application/json");
});

app.Run("http://localhost:5000");

