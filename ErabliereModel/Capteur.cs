using ErabliereApi.Donnees.Ownable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ErabliereApi.Donnees;

/// <summary>
/// Représente un capteur
/// </summary>
public class Capteur : IIdentifiable<Guid?, Capteur>, IErabliereOwnable
{
    /// <summary>
    /// L'id de l'occurence
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// Date de l'ajout du capteur
    /// </summary>
    public DateTimeOffset? DC { get; set; }

    /// <summary>
    /// Indicateur permettant d'afficher ou non le graphique relié au capteur.
    /// </summary>
    public bool AfficherCapteurDashboard { get; set; }

    /// <summary>
    /// Indicateur permettant d'indiquer si les données sont entré depuis un interface graphique
    /// </summary>
    public bool AjouterDonneeDepuisInterface { get; set; }

    /// <summary>
    /// Id de dl'érablière relier a cette donnée
    /// </summary>
    public Guid? IdErabliere { get; set; }

    /// <summary>
    /// L'érablière de ce capteur
    /// </summary>
    public Erabliere? Erabliere { get; set; }

    /// <summary>
    /// Les données du capteurs
    /// </summary>
    public List<DonneeCapteur> DonneesCapteur { get; set; } = new();

    /// <summary>
    /// Les alertes du capteur
    /// </summary>
    public List<AlerteCapteur> AlertesCapteur { get; set; } = new();

    /// <summary>
    /// Le nom donné au capteur
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string? Nom { get; set; }

    /// <summary>
    /// Indice de l'ordre du tri
    /// </summary>
    public int? IndiceOrdre { get; set; }

    /// <summary>
    /// Le symbole qui représente l'unité observer par le capteur.
    /// </summary>
    /// <example>
    /// "°c" pour représenter la temperature en celcius
    /// </example>
    [MaxLength(5)]
    public string? Symbole { get; set; }

    /// <summary>
    /// la string bootstrap pour chnager les dimensions du graphique
    /// </summary>
    public string? Dimension { get; set; }

    /// <inheritdoc />
    public int CompareTo(Capteur? other)
    {
        return Nom?.CompareTo(other?.Nom) ?? 1;
    }
}
