using ErabliereApi.Depot;
using ErabliereApi.Donnees;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ErabliereApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ErablieresController
    {
        private readonly Depot<Erablieres> _dépôt;

        public ErablieresController(Depot<Erablieres> dépôt)
        {
            _dépôt = dépôt;
        }

        [HttpGet]
        public IEnumerable<Erablieres> Lister()
        {
            return _dépôt.Lister();
        }

        [HttpPost]
        public void Ajouter(Erablieres érablières)
        {
            _dépôt.Ajouter(érablières);
        }
    }
}
