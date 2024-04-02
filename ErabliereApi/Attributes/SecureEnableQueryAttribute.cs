using ErabliereApi.Attributes.Validators;
using Microsoft.AspNetCore.OData.Query;

namespace ErabliereApi.Attributes
{
    /// <summary>
    /// Attribut permettant de restreindre l'accès aux données privées contenues dans les notes, documentation et alertes.
    /// </summary>
    public class SecureEnableQueryAttribute : EnableQueryAttribute
    {
        public override void ValidateQuery(HttpRequest request, ODataQueryOptions queryOpts)
        {
            queryOpts.SelectExpand.Validator = new SecureExpandValidator();
            base.ValidateQuery(request, queryOpts);
        }
    }
}
