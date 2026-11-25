using System.Text.Json;
using System.Text.Json.Serialization;
using Crossendor.Api;
using Crossendor.Api.Data;
using Crossendor.Api.Endpoints;
using Crossendor.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// `dotnet run` serves http://localhost:5310 with zero configuration. Hosts that set their own binding
// (containers and cloud hosts via ASPNETCORE_URLS / ASPNETCORE_HTTP_PORTS / --urls) keep theirs.
if (new[] { "urls", "http_ports", "https_ports" }.All(k => string.IsNullOrWhiteSpace(builder.Configuration[k])))
    builder.WebHost.UseUrls("http://localhost:5310");

// camelCase properties and camelCase enums as strings.
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddAppDatabase();
builder.AddPipeline();
builder.AddEdition(); // what differs between the open-source edition and Crossendor Cloud: see Edition.cs

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseEdition();
await app.InitializeDatabaseAsync();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

var api = app.MapGroup("/api");
api.MapSystemEndpoints();
api.MapShopEndpoints();
api.MapEditionEndpoints();

app.Run();
