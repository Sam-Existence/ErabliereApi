using System;
using System.ComponentModel.DataAnnotations;

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

    /// <summary>
    /// Indice du tri
    /// </summary>
    public int? IndiceOrdre { get; set; }

    /// <summary>
    /// Byte qui représente la taille du graphique
    /// </summary>
    [Range(1, 12, ErrorMessage = "La taille du graphique doit être compris entre 1 et 12")]
    public byte? Taille { get; set; }
}
