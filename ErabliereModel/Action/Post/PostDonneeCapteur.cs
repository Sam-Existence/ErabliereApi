using System;

namespace ErabliereApi.Donnees.Action.Post
{
    public class PostDonneeCapteur
    {
        public short V { get; set; }

        public DateTimeOffset? D { get; set; }

        public Guid? IdCapteur { get; set; }
    }
}
