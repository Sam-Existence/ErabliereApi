using AutoFixture;
using ErabliereApi.Attributes;
using ErabliereApi.Depot.Sql;
using ErabliereApi.Test.Autofixture;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Xunit;

namespace ErabliereApi.Test
{
    public class ValiderIPRulesAttributeTest
    {
        [Theory, AutoApiData]
        public void ValiderIPRulesAttributes_SetOrder_OrderIsSet(int order)
        {
            var attribute = new ValiderIPRulesAttribute(order);

            attribute.Order.ShouldBe(order);
        }

        [Theory, AutoApiData]
        public void OnActionExecuting_AucuneAdresseIp_InvalidOperationException(ValiderIPRulesAttribute attribute,
                                                                                    ActionExecutingContext context,
                                                                                    ErabliereDbContext dbContext)
        {
            var erabliere = dbContext.Erabliere.First();
            context.ActionArguments["id"] = erabliere.Id;

            var exception = Should.Throw<InvalidOperationException>(() => attribute.OnActionExecuting(context));

            exception.Message.ShouldBe("Aucune adresse ip distante trouvé.");
        }

        [Theory, AutoApiData]
        public void OnActionExecuting_SelfHostedInvalidIP_ValidationCOnfigAutofixture(ValiderIPRulesAttribute attribute, 
                                                                                    ActionExecutingContext context,
                                                                                    ErabliereDbContext dbContext,
                                                                                    IFixture fixture)
        {
            var erabliere = dbContext.Erabliere.First();
            context.ActionArguments["id"] = erabliere.Id;
            context.HttpContext.Connection.RemoteIpAddress = fixture.CreateRandomIPAddress();

            attribute.OnActionExecuting(context);

            context.ModelState.ErrorCount.ShouldBe(1);
        }

        [Theory, AutoApiData]
        public void OnActionExecuting_ExecutionDeriereReverseProxy_ValidationCOnfigAutofixture(ValiderIPRulesAttribute attribute,
                                                                                               ActionExecutingContext context,
                                                                                               ErabliereDbContext dbContext,
                                                                                               IPAddress adresse)
        {
            var erabliere = dbContext.Erabliere.First();
            context.ActionArguments["id"] = erabliere.Id;
            context.HttpContext.Request.Headers.ContainsKey(Arg.Is("X-Real-IP")).Returns(true);
            context.HttpContext.Request.Headers[Arg.Is("X-Real-IP")].Returns(new StringValues(adresse.ToString()));

            attribute.OnActionExecuting(context);

            context.ModelState.ErrorCount.ShouldBe(1);
        }

        [Theory, AutoApiData]
        public void OnActionExecuting_ExecutionDeriereReverseProxyPlusieursREALIP_ValidationCOnfigAutofixture(ValiderIPRulesAttribute attribute,
                                                                                                              ActionExecutingContext context,
                                                                                                              ErabliereDbContext dbContext,
                                                                                                              List<IPAddress> adresses)
        {
            var erabliere = dbContext.Erabliere.First();
            context.ActionArguments["id"] = erabliere.Id;
            context.HttpContext.Request.Headers.ContainsKey(Arg.Is("X-Real-IP")).Returns(true);
            context.HttpContext.Request.Headers[Arg.Is("X-Real-IP")].Returns(new StringValues(adresses.Select(ip => ip.ToString()).ToArray()));

            attribute.OnActionExecuting(context);

            context.ModelState.ErrorCount.ShouldBe(1);
            context.ModelState["X-Real-IP"].Errors.Single().ErrorMessage.ShouldBe("Une seule entête 'X-Real-IP' doit être trouvé dans la requête.");
        }

        [Theory, AutoApiData]
        public void OnActionExecuting_ExecutionNominale_ValidationCOnfigAutofixture(ValiderIPRulesAttribute attribute,
                                                                                    ActionExecutingContext context,
                                                                                    ErabliereDbContext dbContext)
        {
            var erabliere = dbContext.Erabliere.First();
            context.ActionArguments["id"] = erabliere.Id;
            context.HttpContext.Connection.RemoteIpAddress = new IPAddress(erabliere.IpRule.Split('.').Select(b => byte.Parse(b)).ToArray());

            attribute.OnActionExecuting(context);

            context.ModelState.ErrorCount.ShouldBe(0);
        }

        [Theory, AutoApiData]
        public void OnActionExecuting_ExecutionDeriereReverseREALIPIdentique_ValidationCOnfigAutofixture(ValiderIPRulesAttribute attribute,
                                                                                                              ActionExecutingContext context,
                                                                                                              ErabliereDbContext dbContext)
        {
            var erabliere = dbContext.Erabliere.First();
            context.ActionArguments["id"] = erabliere.Id;
            context.HttpContext.Request.Headers.ContainsKey(Arg.Is("X-Real-IP")).Returns(true);
            context.HttpContext.Request.Headers[Arg.Is("X-Real-IP")].Returns(new StringValues(erabliere.IpRule));

            attribute.OnActionExecuting(context);

            context.ModelState.ErrorCount.ShouldBe(0);
        }

        // TODO : Ajouter des tests pour le cas ou il y a plusieurs ip séparé par des ';'
    }
}
