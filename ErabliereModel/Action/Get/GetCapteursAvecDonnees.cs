using System;
using System.Collections.Generic;

namespace ErabliereApi.Donnees.Action.Get;

/// <summary>
/// Modèle de retour utiliser dans la projection de l'obtention du dashboard
/// </summary>
public class GetCapteursAvecDonnees
{
    /// <summary>
    /// L'id du capteur
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// Le nom du capteur
    /// </summary>
    public string? Nom { get; set; }

    /// <summary>
    /// Indicateur si les données sont ajouter depuis un interface graphique
    /// </summary>
    public bool AjouterDonneeDepuisInterface { get; set; }

    /// <summary>
    /// L'id de l'érablière possédant le capteur
    /// </summary>
    public Guid? IdErabliere { get; set; }

    /// <summary>
    /// La date de création de l'entité.
    /// </summary>
    public DateTimeOffset? DC { get; set; }

    /// <summary>
    /// La liste des données du capteur
    /// </summary>
    public List<GetDonneesCapteur>? Donnees { get; set; }
}
