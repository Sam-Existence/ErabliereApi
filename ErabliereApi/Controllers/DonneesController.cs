using AutoMapper;
using ErabliereApi.Attributes;
using ErabliereApi.Depot;
using ErabliereApi.Donnees;
using ErabliereApi.Donnees.Action.Get;
using ErabliereApi.Donnees.Action.Post;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ErabliereApi.Controllers
{
    /// <summary>
    /// Contrôler représentant les données reçu par l'automate principale
    /// </summary>
    [ApiController]
    [Route("erablieres/{id}/[controller]")]
    public class DonneesController : ControllerBase
    {
        private readonly Depot<Donnee> _depot;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructeur par initlisation
        /// </summary>
        /// <param name="depot"></param>
        /// <param name="mapper">Mapper entre les modèles</param>
        public DonneesController(Depot<Donnee> depot, IMapper mapper)
        {
            _depot = depot;
            _mapper = mapper;
        }

        /// <summary>
        /// Lister les données
        /// </summary>
        /// <param name="id">L'identifiant de l'érablière</param>
        /// <param name="dd">Date de début</param>
        /// <param name="df">Date de début</param>
        /// <param name="q">Quantité de donnée demander</param>
        /// <param name="o">Doit être croissant "c" ou decroissant "d". Par défaut "c"</param>
        /// <response code="200">Retourne une liste de données. La liste est potentiellement vide.</response>
        [ProducesResponseType(200, Type = typeof(GetDonnee))]
        [HttpGet]
        public IActionResult Lister(int id, DateTimeOffset? dd, DateTimeOffset? df, int? q, string? o = "c")
        {
            var query = _depot.Lister(d => d.IdErabliere == id &&
                                      (dd == null || d.D >= dd) &&
                                      (df == null || d.D <= df))
                              .Select(d => new { d.Id, d.D, d.T, d.V, d.NB, d.Iddp, d.Nboc, d.PI });

            if (o == "d")
            {
                query = query.OrderByDescending(d => d);
            }

            if (q.HasValue)
            {
                query = query.Take(q.Value);
            }

            return Ok(query);
        }

        /// <summary>
        /// Ajouter un donnée.
        /// </summary>
        /// <param name="id">L'identifiant de l'érablière</param>
        /// <param name="donneeRecu">La donnée à ajouter</param>
        [HttpPost]
        [ValiderIPRules]
        public async Task<IActionResult> Ajouter(int id, PostDonnee donneeRecu)
        {
            if (id != donneeRecu.IdErabliere)
            {
                return BadRequest($"L'id de la route '{id}' ne concorde pas avec l'id de l'érablière dans la donnée '{donneeRecu.IdErabliere}'.");
            }

            if (donneeRecu.D == default || donneeRecu.D.Equals(DateTimeOffset.MinValue))
            {
                donneeRecu.D = DateTimeOffset.Now;
            }

            var donnePlusRecente = _depot.Lister(d => d.IdErabliere == id).LastOrDefault();

            if (donnePlusRecente != null &&
                donnePlusRecente.IdentiqueMemeLigneDeTemps(donneeRecu))
            {
                var interval = donneeRecu.D.Value - donnePlusRecente.D.Value;

                if (donnePlusRecente.Iddp != null)
                {
                    donnePlusRecente.D = donneeRecu.D;

                    if (donnePlusRecente.PI.HasValue == false)
                    {
                        if (interval.Milliseconds > donnePlusRecente.PI)
                        {
                            donnePlusRecente.PI = (int)interval.TotalSeconds;
                        }
                    }
                    else
                    {
                        donnePlusRecente.PI = (int)interval.TotalSeconds;
                    }

                    donnePlusRecente.Nboc++;

                    await _depot.ModifierAsync(donnePlusRecente);
                }
                else
                {
                    var donnee = _mapper.Map<Donnee>(donneeRecu);

                    donnee.Iddp = donnePlusRecente.Id;

                    donnee.PI = (int)interval.TotalSeconds;

                    await _depot.AjouterAsync(donnee);
                }
            }
            else
            {
                await _depot.AjouterAsync(_mapper.Map<Donnee>(donneeRecu));
            }

            return Ok();
        }
    }
}
