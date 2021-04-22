using System;
namespace ErabliereApi.Donnees
{
    public class GetDonneesCapteur
    {
        public Guid? Id { get; set; }

        public short? Valeur { get; set; }

        /// <summary>
        /// La date de création
        /// </summary>
        public DateTimeOffset? D { get; set; }
    }
}
