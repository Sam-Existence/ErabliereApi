using AutoMapper;
using ErabliereApi.Depot;
using ErabliereApi.Donnees;
using ErabliereApi.Donnees.Action.Post;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ErabliereApi.Controllers
{
    /// <summary>
    /// Contrôler représentant les données reçu par l'automate principale
    /// </summary>
    [ApiController]
    [Route("erablieres/{id}/[controller]")]
    public class DonneesController : ControllerBase
    {
        private readonly Depot<Donnee> dépôt;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructeur par initlisation
        /// </summary>
        /// <param name="dépôt"></param>
        public DonneesController(Depot<Donnee> dépôt, IMapper mapper)
        {
            this.dépôt = dépôt;
            _mapper = mapper;
        }

        /// <summary>
        /// Lister les données
        /// </summary>
        /// <param name="id">L'identifiant de l'érablière</param>
        /// <param name="dd">Date de début</param>
        /// <param name="df">Date de début</param>
        /// <param name="q">Quantité de donnée demander</param>
        /// <param name="t">Trie</param>
        /// <param name="o">Doit être croissant "c" ou decroissant "d". Par défaut "c"</param>
        /// <returns>Liste des données</returns>
        [HttpGet]
        public IEnumerable<Donnee> Lister([DefaultValue("0")] int id, DateTime? dd, DateTime? df, int? q, string? o = "c")
        {
            IEnumerable<Donnee> query = dépôt.Lister(d => d.IdErabliere == id &&
                                                    (dd == null || d.D >= dd) &&
                                                    (df == null || d.D <= df));

            if (o == "d")
            {
                query = query.OrderByDescending(d => d);
            }

            if (q.HasValue)
            {
                query = query.Take(q.Value);
            }

            return query;
        }

        /// <summary>
        /// Ajouter un donnée
        /// </summary>
        /// <param name="id">L'identifiant de l'érablière</param>
        /// <param name="donneeRecu">La donnée à ajouter</param>
        [HttpPost]
        public IActionResult Ajouter([DefaultValue(0)] int id, PostDonnee donneeRecu)
        {
            if (id != donneeRecu.IdErabliere)
            {
                return BadRequest($"L'id de la route '{id}' ne concorde pas avec l'id de l'érablière dans la donnée '{donneeRecu.IdErabliere}'.");
            }

            if (donneeRecu.D == default || donneeRecu.D.Equals(DateTime.MinValue))
            {
                donneeRecu.D = DateTime.Now;
            }

            var donnePlusRecente = dépôt.Lister(d => d.IdErabliere == id).LastOrDefault();

            if (donnePlusRecente != null &&
                donnePlusRecente.NB == donneeRecu.NB &&
                donnePlusRecente.T == donneeRecu.T &&
                donnePlusRecente.V == donneeRecu.V &&
                donneeRecu.D.HasValue && donnePlusRecente.D.HasValue &&
                donneeRecu.D.Value > donnePlusRecente.D.Value)
            {
                if (donnePlusRecente.IdDonneePrecedente != null)
                {
                    var interval = donneeRecu.D.Value - donnePlusRecente.D.Value;

                    donnePlusRecente.D = donneeRecu.D;

                    if (donnePlusRecente.PI.HasValue == false)
                    {
                        if (interval > donnePlusRecente.PI)
                        {
                            donnePlusRecente.PI = interval;
                        }
                    }
                    else
                    {
                        donnePlusRecente.PI = interval;
                    }

                    donnePlusRecente.Nboc++;

                    dépôt.Modifier(donnePlusRecente);
                }
                else
                {
                    var donnee = _mapper.Map<Donnee>(donneeRecu);

                    donnee.IdDonneePrecedente = donnePlusRecente.Id;

                    dépôt.Ajouter(donnee);
                }
            }
            else
            {
                dépôt.Ajouter(_mapper.Map<Donnee>(donneeRecu));
            }

            return Ok();
        }
    }
}
