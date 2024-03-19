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

    /// <summary>
    /// Numéros de téléphone dans un tableau de chaîne de caractère
    /// </summary>
    public string[] Numeros { get; set; } = System.Array.Empty<string>();
}
