using System;

namespace ErabliereApi.Donnees.Action.Delete;

/// <summary>
/// Modèle de suppression d'un capteur
/// </summary>
public class DeleteCapteur
{
    /// <summary>
    /// L'id du capteur
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// L'id de l'érablière possédant le capteur
    /// </summary>
    public Guid IdErabliere { get; set; }
}
