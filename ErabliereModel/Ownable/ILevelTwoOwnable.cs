using System;

namespace ErabliereApi.Donnees.Ownable;
internal interface ILevelTwoOwnable<T> : IOwnable where T : class, IErabliereOwnable
{
    public T? Owner { get; set; }

    public Guid? OwnerId { get; set; }
}
