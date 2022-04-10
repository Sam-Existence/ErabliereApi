namespace ErabliereApi.Donnees.Action.Get;

/// <summary>
/// Modèle reçu lors de l'obtention des alertes
/// </summary>
public class GetAlerte : Alerte
{
    /// <summary>
    /// Adresse couriels dans un tableau de chaîne de caractère
    /// </summary>
    public string[] Emails { get; set; } = System.Array.Empty<string>();
}
