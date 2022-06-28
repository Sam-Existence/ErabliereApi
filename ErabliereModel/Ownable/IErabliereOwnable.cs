using System;

namespace ErabliereApi.Donnees.Ownable;

/// <summary>
/// Interface indiquant la relation entre une érablière une autre entité
/// </summary>
public interface IErabliereOwnable : IOwnable
{
    /// <summary>
    /// La clé étrangère de l'érablière
    /// </summary>
    public Guid? IdErabliere { get; set; }

    /// <summary>
    /// L'érablière qui possède la note
    /// </summary>
    public Erabliere? Erabliere { get; set; }
}
