using AutoMapper;
using ErabliereApi.Controllers;
using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;
using ErabliereApi.Donnees.Action.Post;
using ErabliereApi.Test.Autofixture;
using ErabliereApi.Test.EqualityComparer;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace ErabliereApi.Test
{
    public class ErabliereControllerTest
    {
        private readonly JsonComparer<object> _ignoreIdsEqualityComparer;

        public ErabliereControllerTest()
        {
            _ignoreIdsEqualityComparer = new JsonComparer<object>();
        }

        [Theory, AutoApiData]
        public async Task Ajouter_Erabliere_CountErabliereIncrementerDeUn(ErablieresController erabliereController, 
                                                                          PostErabliere postErabliere, 
                                                                          ErabliereDbContext erabliereDbContext)
        {
            var initialCount = erabliereDbContext.Erabliere.Count();

            await erabliereController.Ajouter(postErabliere);

            erabliereDbContext.Erabliere.Count().ShouldBe(initialCount + 1);
        }

        [Theory, AutoApiData]
        public async Task Ajouter_Erabliere_DbSetContientErabliere(ErablieresController erabliereController, 
                                                                   PostErabliere postErabliere, 
                                                                   ErabliereDbContext erabliereDbContext, 
                                                                   IMapper mapper)
        {
            var initialSet = new HashSet<Erabliere>(erabliereDbContext.Erabliere);

            await erabliereController.Ajouter(postErabliere);

            initialSet.ShouldNotContain(mapper.Map<Erabliere>(postErabliere), _ignoreIdsEqualityComparer);

            string customMessageFunc()
            {
                var sb = new StringBuilder();

                sb.AppendLine("erabliereDbContext.Erabliere.ShouldContain");
                sb.AppendLine(JsonSerializer.Serialize(mapper.Map<Erabliere>(postErabliere), _ignoreIdsEqualityComparer.JsonSerializerOptions));
                sb.AppendLine("But was actually");
                foreach (var e in erabliereDbContext.Erabliere)
                {
                    sb.AppendLine(JsonSerializer.Serialize(e, _ignoreIdsEqualityComparer.JsonSerializerOptions));
                }

                return sb.ToString();
            }

            var erabliereSansIds = erabliereDbContext.Erabliere.Select(e => mapper.Map<PostErabliere>(e));

            erabliereSansIds.ShouldContain(postErabliere, _ignoreIdsEqualityComparer, customMessageFunc());
        }

        [Theory, AutoApiData]
        public async Task Ajouter_Erabliere_NePeutPasAvoirDeuxFoisLeMemeNom(ErablieresController erabliereController, 
                                                                            PostErabliere postErabliere, 
                                                                            ErabliereDbContext erabliereDbContext,
                                                                            IMapper mapper)
        {
            var initialCount = erabliereDbContext.Erabliere.Count();
            var bdErabliere = erabliereDbContext.Erabliere.First();

            postErabliere.Nom = bdErabliere.Nom;

            await erabliereController.Ajouter(postErabliere);

            erabliereDbContext.Erabliere.Count().ShouldBe(initialCount);

            var erabliereSansIds = erabliereDbContext.Erabliere.Select(e => mapper.Map<PostErabliere>(e));

            erabliereSansIds.ShouldNotContain(postErabliere, _ignoreIdsEqualityComparer);
        }
    }
}
