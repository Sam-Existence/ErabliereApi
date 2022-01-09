namespace ErabliereApi.Donnees.Generic;

/// <summary>
/// Classe représentant une pair pour facilité la sérialisation
/// </summary>
/// <typeparam name="K"></typeparam>
/// <typeparam name="V"></typeparam>
public class Pair<K, V>
{
    /// <summary>
    /// Constructeur par défaut
    /// </summary>
    /// <param name="id"></param>
    /// <param name="valeur"></param>
    public Pair(K id, V valeur)
    {
        Id = id;
        Valeur = valeur;
    }

    /// <summary>
    /// La clé
    /// </summary>
    public K Id { get; set; }

    /// <summary>
    /// La valeur
    /// </summary>
    public V Valeur { get; set; }
}
