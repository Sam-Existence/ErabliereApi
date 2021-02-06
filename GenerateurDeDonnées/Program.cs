using ErabliereApi.Donnees;
using System;
using System.Net.Http;

namespace GenerateurDeDonnées
{
    class Program
    {
        static HttpClient _client;
        static string _url = "http://localhost:5000";
        static int _nombreErabliere = 2;
        static int _nombreBarils = 30;
        static int _nombreDompeux = 200;
        static int _nombreDonnees = 500;
        static DateTime _debutCollecte = new DateTime(2021, 03, 15);
        static DateTime _finCollecte = new DateTime(2021, 04, 29);
        static Random _random = new Random();
        static Modelisateur _modelisateur = new Modelisateur();

        static void Main(string[] args)
        {
            Console.WriteLine("Générateur de données - Érablière API");
            Console.WriteLine();

            _client = new HttpClient();

            GénérerErabliere();
            GénérerBarils();
            GénérerDompeux();
            GénérerDonnees();
        }

        private static void GénérerDonnees()
        {
            Console.WriteLine(nameof(GénérerDonnees));

            for (int i = 0; i < _nombreErabliere; i++)
            {
                for (int j = 0; j < _nombreDonnees; j++)
                {
                    CreerDonnees($"/erablieres/{i}/donnees", NouvelleDonnee(i, j));
                }
            }
        }

        private static HttpContent NouvelleDonnee(int i, int j)
        {
            var d = _debutCollecte + TimeSpan.FromSeconds(30 * j);

            var donnee = new Donnee
            {
                IdErabliere = i,
                D = d,
                NB = (short)(Math.Sin(j) * 100),
                T = _modelisateur.Temperature(d),
                V = (short)(24 + (Math.Sin(j / 4) * 100))
            };

            return donnee.ToStringContent();
        }

        private static void GénérerDompeux()
        {
            Console.WriteLine(nameof(GénérerDompeux));

            for (int i = 0; i < _nombreErabliere; i++)
            {
                for (int j = 0; j < _nombreDompeux; j++)
                {
                    CreerDonnees($"/erablieres/{i}/dompeux", NouveauDompeux(i, j));
                }
            }
        }

        private static HttpContent NouveauDompeux(int i, int j)
        {
            var dompeux = new Dompeux
            {
                IdErabliere = i,
                T = _debutCollecte + TimeSpan.FromMinutes(j * 12)
            };

            return dompeux.ToStringContent();
        }

        private static void GénérerBarils()
        {
            Console.WriteLine(nameof(GénérerBarils));

            for (int i = 0; i < _nombreErabliere; i++)
            {
                for (int j = 0; j < _nombreBarils * (1 + i); j++)
                {
                    CreerDonnees($"/erablieres/{i}/Baril", NouveauBaril(i, j));
                }
            }
        }

        private static HttpContent NouveauBaril(int idErabliere, int x)
        {
            var barils = new Baril
            {
                IdErabliere = idErabliere,
                DF = _debutCollecte + TimeSpan.FromDays(x * 2),
                Id = _random.Next(10000),
                QE = "A"
            };

            return barils.ToStringContent();
        }

        private static void GénérerErabliere()
        {
            Console.WriteLine(nameof(GénérerErabliere));

            for (int i = 0; i < _nombreErabliere; i++)
            {
                CreerDonnees("/erablieres", NouvelleErabliere(i));
            }
        }

        private static HttpContent NouvelleErabliere(int i)
        {
            var erabliere = new Erablieres()
            {
                Nom = $"Érablière {i}"
            };

            return erabliere.ToStringContent();
        }

        private static void CreerDonnees(string action, HttpContent data)
        {
            var url = $"{_url}{action}";

            Console.WriteLine(url);

            var response = _client.PostAsync(url, data).Result;

            Console.WriteLine(response.StatusCode);
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        }
    }
}
