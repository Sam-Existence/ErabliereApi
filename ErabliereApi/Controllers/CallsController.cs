using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ErabliereApi.Controllers;

/// <summary>
/// Contrôler représentant les données des dompeux
/// </summary>
[ApiController]
[Route("[controller]/[action]")]
[Authorize(Roles = "ErabliereCalls", Policy = "TenantIdPrincipal")]
public class CallsController : ControllerBase
{
    private readonly IConfiguration _config;

    /// <summary>
    /// Calls controller, allow users to call each other
    /// </summary>
    public CallsController(IConfiguration config)
    {
        _config = config;
    }

    /// <summary>
    /// Get the appId use to initialize client
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetAppId(CancellationToken token)
    {
        using var client = new HttpClient();

        var localBackend = _config["agora.localbackend"]?.Trim();

        if (localBackend != null && localBackend.EndsWith('/'))
        {
            localBackend = localBackend.Remove(localBackend.Length - 1);
        }

        var tokenResponse = await client.GetAsync($"{localBackend}/accessToken/appId/view", token);

        return Ok(await tokenResponse.Content.ReadFromJsonAsync(typeof(object), token));
    }


    /// <summary>
    /// Get an access token
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="channel"></param>
    /// <param name="role"></param>
    /// <param name="tokenType"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetAccessToken([FromQuery] int? uid, [FromQuery] string? channel, [FromQuery] string? role, [FromQuery] string? tokenType, CancellationToken token)
    {
        using var client = new HttpClient();

        var localBackend = _config["agora.localbackend"]?.Trim();

        if (localBackend != null && localBackend.EndsWith('/'))
        {
            localBackend = localBackend.Remove(localBackend.Length - 1);
        }

        var tokenResponse = await client.GetAsync($"{localBackend}/accessToken/{uid}?channel={channel}&tokenType={tokenType}", token);

        return Ok(await tokenResponse.Content.ReadFromJsonAsync(typeof(object), token));
    }
}
