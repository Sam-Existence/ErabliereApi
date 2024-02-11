using System;

namespace ErabliereApi.Donnees.Action.Put;

/// <summary>
/// Modèle de modification d'un capteur
/// </summary>
public class PutCapteur
{
    /// <summary>
    /// L'id du capteur à modifier
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// Le nom du capteur
    /// </summary>
    public string? Nom { get; set; }

    /// <summary>
    /// Le symbole du capteur
    /// </summary>
    public string? Symbole { get; set; }

    /// <summary>
    /// L'id de l'érablière
    /// </summary>
    public Guid? IdErabliere { get; set; }

    /// <summary>
    /// Indicateur permettant d'afficher ou non le graphique relié au capteur.
    /// </summary>
    public bool? AfficherCapteurDashboard { get; set; }

    /// <summary>
    /// Indicateur permettant d'indiquer si les données sont entré depuis un interface graphique
    /// </summary>
    public bool? AjouterDonneeDepuisInterface { get; set; }

    /// <summary>
    /// La date de création de l'entité.
    /// </summary>
    public DateTimeOffset? DC { get; set; }
}
