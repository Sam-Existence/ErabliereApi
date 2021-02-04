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
            var x = dateTime.Month;

            static double f(double x) => 4.2 * Math.Sin((x + 1) * Math.PI / 6) + 13.7;

            var y = f(x);

            return (short)y;
        }
    }
}
