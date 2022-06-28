using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErabliereApi.Donnees.Action.NonHttp;

public class CustomerErabliereOwnershipAccess
{
    /// <summary>
    /// Clé primaire de la jonction
    /// </summary>
    public Guid? Id { get; set; }

    /// <inheritdoc cref="Donnees.CustomerErabliere.Access" />
    public byte Access { get; set; }
}
