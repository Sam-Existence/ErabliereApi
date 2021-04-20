using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErabliereApi.Donnees.Action.Post
{
    public class PostDonneeCapteur
    {
        public short V { get; set; }

        public DateTimeOffset? D { get; set; }

        public int? IdCapteur { get; set; }
    }
}
