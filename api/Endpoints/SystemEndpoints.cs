namespace Crossendor.Api.Endpoints;

public static class SystemEndpoints
{
    public static void MapSystemEndpoints(this RouteGroupBuilder api)
    {
        // What this instance runs with. Says whether a key is set, never the key.
        api.MapGet("/system", (IConfiguration config) => Results.Ok(new
        {
            edition = Edition.Name,
            mock = config.GetValue("AI:UseMockData", true),
            anthropicKey = !string.IsNullOrWhiteSpace(config["AI:Anthropic:ApiKey"]),
        }));
    }
}
