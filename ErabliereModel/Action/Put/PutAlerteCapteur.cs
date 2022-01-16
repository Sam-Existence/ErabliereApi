using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErabliereApi.Donnees.Action.Put;

/// <summary>
/// Modèle pour la modification d'une alerte d'un capteur
/// </summary>
public class PutAlerteCapteur
{
    /// <summary>
    /// La clé primaire
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// L'id du capteur
    /// </summary>
    public Guid? IdCapteur { get; set; }

    /// <summary>
    /// Une liste d'adresse email séparer par des ';'
    /// </summary>
    /// <example>exemple@courriel.com;exemple2@courriel.com</example>
    [MaxLength(200)]
    public string? EnvoyerA { get; set; }

    /// <summary>
    /// La valeur minimal de ce capteur
    /// </summary>
    public short? MinVaue { get; set; }

    /// <summary>
    /// La valeur maximal de ce capteur
    /// </summary>
    public short? MaxValue { get; set; }
}
