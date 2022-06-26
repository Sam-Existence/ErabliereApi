using System;

namespace ErabliereApi.Donnees;

/// <summary>
/// Jonction entre les érablières et les utilisateurs
/// </summary>
public class CustomerErabliere : IIdentifiable<Guid?, CustomerErabliere>
{
    /// <summary>
    /// Clé primaire de la jonction
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// Id de l'érablière
    /// </summary>
    public Guid? IdErabliere { get; set; }

    /// <summary>
    /// L'érablière
    /// </summary>
    public Erabliere? Erabliere { get; set; }

    /// <summary>
    /// Id de l'utilisateur
    /// </summary>
    public Guid? IdCustomer { get; set; }

    /// <summary>
    /// L'utilisateur
    /// </summary>
    public Customer? Customer { get; set; }

    /// <summary>
    /// Indique les accès relié à l'utilisateur et l'érablière
    /// 
    /// Les droits utilise les 4 premier bit d'un octet pour établiere si l'utilisateur
    /// à les droits pour l'action effectué.
    /// 
    /// 0  (0000): Aucun droit
    /// 1  (0001): GET
    /// 2  (0010): POST
    /// 3  (0011): GET + POST
    /// 4  (0100): PUT
    /// 5  (0101): GET + PUT
    /// 6  (0110): POST + PUT
    /// 7  (0111): GET + POST + PUT
    /// 8  (1000): DELETE
    /// 9  (1001): GET + DELETE
    /// 10 (1010): POST + DELETE
    /// 11 (1011): GET + POST + DELETE
    /// 12 (1100): PUT + DELETE
    /// 13 (1101): GET + PUT + DELETE
    /// 14 (1110): POST + PUT + DELETE
    /// 15 (1111): GET + POST + PUT + DELETE
    /// </summary>
    public byte Access { get; set; }

    /// <inheritdoc />
    public int CompareTo(CustomerErabliere? other)
    {
        return Access.CompareTo(other?.Access ?? 0);
    }
}
