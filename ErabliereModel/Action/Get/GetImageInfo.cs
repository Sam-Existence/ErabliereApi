using System;
using System.ComponentModel.DataAnnotations;

namespace ErabliereApi.Donnees.Action.Get;

public class GetImageInfo
{
    public long Id { get; set; }

    public uint UniqueId { get; set; }

    public string? Name { get; set; }

    /// <summary>
    /// Object from the email of the image
    /// </summary>
    [MaxLength(400)]
    public string? Object { get; set; }

    public string? AzureImageAPIInfo { get; set; }

    public byte[]? Images { get; set; }

    public DateTimeOffset DateAjout { get; set; }

    public DateTimeOffset? DateEmail { get; set; }

    public Guid? EmailStatesId { get; set; }

    public Guid? ExternalOwner { get; set; }
}
