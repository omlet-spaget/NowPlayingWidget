using Microsoft.AspNetCore.Mvc;
using NowPlayingWidget.Service;
using NowPlayingWidget.Service.GlobalSystemMediaTransportControls;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    WebRootPath = Path.Combine(Environment.CurrentDirectory, "html")
});

// Add services to the container.
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddSingleton<INowPlayingService, NowPlayingService>();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseDefaultFiles();
app.UseStaticFiles();


app.MapGet("/media-info", async ([FromServices] INowPlayingService svc) =>
{
    return await svc.GetMediaInfo();
});

app.MapGet("/thumbnail", async ([FromServices] INowPlayingService svc, string a, string t) =>
{
    var tbn = await svc.GetThumbnail();
    if (tbn is null || tbn.Artist != a || tbn.Title != t)
    {
        return Results.BadRequest();
    }
    var content = await tbn.GetStreamAsync();

    if (content is null)
        return Results.NoContent();

    return Results.File(content.Stream, content.ContentType);
});

app.Run();


[JsonSerializable(typeof(MediaInfo))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}