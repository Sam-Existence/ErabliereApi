using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErabliereApi.Donnees.Action.Put;

/// <summary>
/// Classe permettant de modifier un utilisateur
/// </summary>
public class PutCustomer
{
    /// <summary>
    /// Identifiant de l'utilisateur
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nom de l'utilisateur
    /// </summary>
    public string? Name { get; set; }
}
