using ErabliereApi.Attributes.Validators;
using Microsoft.AspNetCore.OData.Query;

namespace ErabliereApi.Attributes
{
    public class SecureEnableQueryAttribute : EnableQueryAttribute
    {
        public override void ValidateQuery(HttpRequest request, ODataQueryOptions queryOpts)
        {
            queryOpts.SelectExpand.Validator = new SecureExpandValidator();
            base.ValidateQuery(request, queryOpts);
        }
    }
}
