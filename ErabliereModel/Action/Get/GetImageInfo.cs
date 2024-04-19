using System;
using System.ComponentModel.DataAnnotations;

namespace ErabliereApi.Donnees.Action.Get;

/// <summary>
/// Information d'une image
/// </summary>
public class GetImageInfo
{
    /// <summary>
    /// Id de l'image
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Id unique de l'image provenant du fournisseur d'email
    /// </summary>
    public uint UniqueId { get; set; }

    /// <summary>
    /// Nom de l'image
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Objet de l'email de l'image
    /// </summary>
    [MaxLength(400)]
    public string? Object { get; set; }

    /// <summary>
    /// Structure JSON retourné par l'API Azure Vision
    /// </summary>
    public string? AzureImageAPIInfo { get; set; }

    /// <summary>
    /// Image
    /// </summary>
    public byte[]? Images { get; set; }

    /// <summary>
    /// Date d'ajout de l'image
    /// </summary>
    public DateTimeOffset DateAjout { get; set; }

    /// <summary>
    /// Date de l'email de l'image
    /// </summary>
    public DateTimeOffset? DateEmail { get; set; }

    /// <summary>
    /// Id de l'email de l'image
    /// </summary>
    public Guid? EmailStatesId { get; set; }

    /// <summary>
    /// Id de l'érablière possédant l'image
    /// </summary>
    public Guid? ExternalOwner { get; set; }
}
