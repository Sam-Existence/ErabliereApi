using System;

namespace GenerateurDeDonnées
{
    public class Modelisateur
    {
        /// <summary>
        /// Donnee une température réaliste en fonction de la date de l'année
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public short Temperature(DateTime dateTime)
        {
            return (short)(63 * Math.Sin((dateTime.DayOfYear / 100.0) + 100) + 
                          (14 * Math.Sin(dateTime.Hour + (dateTime.Minute / 60))));
        }
    }
}
