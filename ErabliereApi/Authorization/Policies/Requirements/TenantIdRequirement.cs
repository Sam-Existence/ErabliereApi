using Microsoft.AspNetCore.Authorization;

namespace ErabliereApi.Authorization.Policies.Requirements;
/// <summary>
/// Requirement qui contient le tenantId du tenant principal
/// </summary>
public class TenantIdRequirement : IAuthorizationRequirement
{
    /// <summary>
    /// Constructeur par initlaisation
    /// </summary>
    /// <param name="tenantId"></param>
    public TenantIdRequirement(string tenantId) => TenantId = tenantId;
    /// <summary>
    /// Le tenantId du tenant principal
    /// </summary>
    public string TenantId { get; }
}
