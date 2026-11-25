using Crossendor.Api.Services;

namespace Crossendor.Api;

/// <summary>
/// The parts that differ between the open-source edition and Crossendor Cloud. This is the open-source one:
/// a single local user, no sign-in, no billing. The cloud repository replaces this file (and adds its own folder),
/// so updates to everything else merge in without conflicts. Keep edition-specific code here, not in shared files.
/// </summary>
public static class Edition
{
    public const string Name = "open-source";

    public static void AddEdition(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ICurrentUser, LocalUser>();
    }

    public static void UseEdition(this WebApplication app)
    {
    }

    public static void MapEditionEndpoints(this RouteGroupBuilder api)
    {
    }

    /// <summary>No sign-in: every request acts as the instance's one local user.</summary>
    private sealed class LocalUser : ICurrentUser
    {
        public string? IdOf(HttpContext context) => "local";
    }
}
