using ErabliereApi.Controllers;
using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees.Action.Get;
using ErabliereApi.Test.Autofixture;
using ErabliereApi.Test.EqualityComparer;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;

namespace ErabliereApi.Test;
public class AlerteCapteurControllerTest
{
    private readonly JsonComparer<object> _ignoreIdsEqualityComparer;

    public AlerteCapteurControllerTest()
    {
        _ignoreIdsEqualityComparer = new JsonComparer<object>();
    }

    [Theory, AutoApiData]
    public async Task TestGetAlerteCapteur(
        AlerteCapteursController controller, ErabliereDbContext context) 
    {
        var erabliere = context.Erabliere.GetRandom();

        var response = await controller.ListerAlerteCapteurErabliere(
            erabliere.Id.Value,
            additionnalProperties: true,
            include: "Capteur",
            System.Threading.CancellationToken.None
        );  

        var alerteCapteurs = Assert.IsType<GetAlerteCapteur[]>(response);

        Assert.NotNull(alerteCapteurs[0].Capteur);
        Assert.NotEmpty(alerteCapteurs[0].Capteur.Symbole);
    }

    [Theory, AutoApiData]
    public async Task TestPutAlerteCapteur(
        AlerteCapteursController controller, ErabliereDbContext context)
    {
        var alerteCapteur = context.AlerteCapteurs.GetRandom();

        Assert.Single(alerteCapteur.EnvoyerA.Split(';'));

        var response = await controller.Modifier(
            alerteCapteur.IdCapteur.Value,
            new Donnees.Action.Put.PutAlerteCapteur
            {
                EnvoyerA = "test@test.com;test1@test.com",
                Id = alerteCapteur.Id,
                IdCapteur = alerteCapteur.IdCapteur,
                IsEnable = alerteCapteur.IsEnable,
                MaxValue = alerteCapteur.MaxValue,
                MinVaue = alerteCapteur.MinVaue,
                Nom = alerteCapteur.Nom
            },
            true,
            System.Threading.CancellationToken.None);

        var result = Assert.IsType<OkObjectResult>(response);

        var alerteCapteurResponse = Assert.IsType<GetAlerteCapteur>(result.Value);

        Assert.Equal(2, alerteCapteurResponse.Emails.Length);
    }
}
