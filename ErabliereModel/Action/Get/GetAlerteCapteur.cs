using System;

namespace ErabliereApi.Donnees.Action.Get;

/// <summary>
/// Modèle des alertes capteur utiliser pour l'obtention des alertes capteur.
/// </summary>
public class GetAlerteCapteur : AlerteCapteur
{
    /// <summary>
    /// La listes des courriels dans une liste
    /// </summary>
    public string[] Emails { get; set; } = Array.Empty<string>();
}
