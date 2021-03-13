using ErabliereApi.Attributes;
using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ErabliereApi.Controllers
{
    /// <summary>
    /// Contrôler représentant les données des dompeux
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AuthorsationController : ControllerBase
    {
        /// <summary>
        /// Contrôleur pour tester l'authentification oauth2 open ID connect
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
