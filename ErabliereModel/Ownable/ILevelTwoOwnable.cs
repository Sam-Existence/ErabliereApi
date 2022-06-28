using System;

namespace ErabliereApi.Donnees.Ownable;

/// <summary>
/// Deuxième niveau de relation d'une entité appartenant à une érablière
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ILevelTwoOwnable<T> : IOwnable where T : class, IErabliereOwnable
{
    /// <summary>
    /// Référence vers le premier niveau dans la hierachie.
    /// </summary>
    public T? Owner { get; set; }

    /// <summary>
    /// L'id de l'entité propriétaire
    /// </summary>
    public Guid? OwnerId { get; set; }
}
