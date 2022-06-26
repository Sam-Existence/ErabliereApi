using System;

namespace ErabliereApi.Donnees.Ownable;
internal interface IErabliereOwnable : IOwnable
{
    /// <summary>
    /// La clé étrangère de l'érablière
    /// </summary>
    public Guid? IdErabliere { get; set; }

    /// <summary>
    /// L'érablière qui possède la note
    /// </summary>
    public Erabliere? Erabliere { get; set; }
}
