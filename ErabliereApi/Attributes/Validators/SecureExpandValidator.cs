
using ErabliereApi.Donnees;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Query.Validator;

namespace ErabliereApi.Attributes.Validators;
/// <summary>
/// Validator qui vérifie que la requête Expand ne contient pas Aletes, Documentations ou Notes.
/// </summary>
public class SecureExpandValidator : SelectExpandQueryValidator
{
    /// <inheritdoc />
    public override void Validate(SelectExpandQueryOption selectExpandQueryOption, ODataValidationSettings validationSettings)
    {
        string expand = selectExpandQueryOption.RawExpand;

        if (expand.Contains(nameof(Erabliere.Alertes), StringComparison.OrdinalIgnoreCase))
        {
            throw new Microsoft.OData.ODataException($"La requête sur {nameof(Erabliere.Alertes)} n'est pas permise");
        }

        if (expand.Contains(nameof(Erabliere.Documentations), StringComparison.OrdinalIgnoreCase))
        {
            throw new Microsoft.OData.ODataException($"La requête sur {nameof(Erabliere.Documentations)} n'est pas permise");
        }

        if (expand.Contains(nameof(Erabliere.Notes), StringComparison.OrdinalIgnoreCase))
        {
            throw new Microsoft.OData.ODataException($"La requête sur {nameof(Erabliere.Notes)} n'est pas permise");
        }

        if (expand.Contains(nameof(Erabliere.CustomerErablieres), StringComparison.OrdinalIgnoreCase))
        {
            throw new Microsoft.OData.ODataException($"La requête sur {nameof(Erabliere.CustomerErablieres)} n'est pas permise");
        }

        base.Validate(selectExpandQueryOption, validationSettings);
    }
}