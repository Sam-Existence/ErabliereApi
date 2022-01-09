using System;

namespace ErabliereApi.Donnees.Action.Put;

/// <summary>
/// Modèle de donnée de modification d'une donnée
/// </summary>
public class PutDonnee
{
    /// <summary>
    /// Id de la donnée à modifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Vaccium en dixième de HG
    /// </summary>
    public short? V { get; set; }

    /// <summary>
    /// Id de dl'érablière relier a cette donnée
    /// </summary>
    public Guid? IdErabliere { get; set; }
}
