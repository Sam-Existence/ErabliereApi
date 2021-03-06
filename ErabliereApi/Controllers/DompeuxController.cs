using AutoMapper;
using ErabliereApi.Attributes;
using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;
using ErabliereApi.Donnees.Action.Get;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ErabliereApi.Controllers
{
    /// <summary>
    /// Contrôler représentant les données des dompeux
    /// </summary>
    [ApiController]
    [Route("erablieres/{id}/[controller]")]
    public class DompeuxController : ControllerBase
    {
        private readonly ErabliereDbContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructeur par initialisation
        /// </summary>
        /// <param name="context">Classe de context pour accéder à la base de donnée</param>
        /// <param name="mapper">Interface de mappagin entre les modèles</param>
        public DompeuxController(ErabliereDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Lister les dompeux
        /// </summary>
        /// <param name="id">Identifiant de l'érablière</param>
        /// <param name="dd">La date de début</param>
        /// <param name="df">La date de fin</param>
        /// <param name="q">La quantité de donnée retourné</param>
        /// <param name="o">L'ordre des dompeux par date d'occurence. "c" = croissant, "d" = décoissant. </param>
        /// <response code="200">Une liste avec les dompeux. La liste est potentiellement vide.</response>
        [HttpGet]
        public IEnumerable<GetDompeux> Lister(int id, DateTimeOffset? dd, DateTimeOffset? df, int? q, string? o = "c")
        {
            var query = _context.Dompeux.AsNoTracking()
                                        .Where(d => d.IdErabliere == id &&
                                               (dd == null || d.T >= dd) &&
                                               (df == null || d.T <= df));

            if (o == "d")
            {
                query = query.Reverse();
            }
            
            if (q.HasValue)
            {
                query = query.Take(q.Value);
            }

            return query.Select(d => _mapper.Map<GetDompeux>(d));
        }

        /// <summary>
        /// Ajouter un dompeux
        /// </summary>
        /// <param name="id">L'identifiant de l'érablière</param>
        /// <param name="dompeux"></param>
        [HttpPost]
        [ValiderIPRules]
        public async Task<IActionResult> Ajouter(int id, Dompeux dompeux)
        {
            if (id != dompeux.IdErabliere)
            {
                return BadRequest("L'id de la route ne concorde pas avec l'id du dompeux");
            }

            if (dompeux.T == default || dompeux.T.Equals(DateTimeOffset.MinValue))
            {
                dompeux.T = DateTimeOffset.Now;
            }

            await _context.Dompeux.AddAsync(dompeux);

            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Modifier un dompeux
        /// </summary>
        /// <param name="id">Identifiant de l'érablière</param>
        /// <param name="idDompeux">L'id du dompeux à modifier</param>
        /// <param name="donnee">Le dompeux à modifier</param>
        [HttpPut("{idDompeux}")]
        [ValiderIPRules]
        public async Task<IActionResult> Modifier(int id, int idDompeux, Dompeux donnee)
        {
            if (id != donnee.IdErabliere)
            {
                return BadRequest("L'id de l'érablière dans la route ne concorde pas l'id de l'érablière dans le dompeux.");
            }
            if (idDompeux != donnee.Id)
            {
                return BadRequest("L'id du dompeux de la route ne concorde pas avec l'id du dompeux");
            }

            _context.Dompeux.Update(donnee);

            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Supprimer un dompeux
        /// </summary>
        /// <param name="id">L'identifiant de l'érablière</param>
        /// <param name="idDompeux">L'id du dompeux à supprimer</param>
        /// <param name="donnee">Le dompeux a supprimer</param>
        [HttpDelete("{idDompeux}")]
        [ValiderIPRules]
        public IActionResult Supprimer(int id, int idDompeux, Dompeux donnee)
        {
            if (id != donnee.IdErabliere)
            {
                return BadRequest("L'id de l'érablière dans la route ne concorde pas l'id de l'érablière dans le dompeux.");
            }
            if (idDompeux != donnee.Id)
            {
                return BadRequest("L'id du dompeux de la route ne concorde pas avec l'id du dompeux");
            }

            _context.Dompeux.Remove(donnee);

            _context.SaveChanges();

            return NoContent();
        }
    }
}
