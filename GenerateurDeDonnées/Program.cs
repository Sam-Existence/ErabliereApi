using ErabliereApi.Donnees;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace GenerateurDeDonnées
{
    class Program
    {
        static HttpClient _client;
        static string _url = "http://localhost:5000";
        static int _nombreErabliere = 2;
        static int _nombreBarils = 30;
        static int _nombreDompeux = 1000;
        static int _nombreDonnees = 51840;
        static Random _random = new Random();

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
                    var url = $"{_url}/erablieres/{i}/donnees";
                    var data = NouvelleDonnees(i, j);

                    Console.WriteLine(url);

                    var response = _client.PostAsync(url, data).Result;

                    Console.WriteLine(response.StatusCode);
                    Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                }
            }
        }

        private static HttpContent NouvelleDonnees(int i, int j)
        {
            var donnee = new Donnee
            {
                IdÉrablière = i,
                D = new DateTime(2021, 03, 15),
                NB = (short)(Math.Sin(j) * 100),
                T = (short)(Math.Sin(j / 2) * 100),
                V =(short)(24 + (Math.Sin(j / 4) * 100))
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
                    var url = $"{_url}/erablieres/{i}/dompeux";
                    var data = NouveauDompeux(i, j);

                    Console.WriteLine(url);

                    var response = _client.PostAsync(url, data).Result;

                    Console.WriteLine(response.StatusCode);
                    Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                }
            }
        }

        private static HttpContent NouveauDompeux(int i, int j)
        {
            var dompeux = new Dompeux
            {
                IdÉrablière = i,
                T = new DateTime(2021, 03, 21) + TimeSpan.FromMinutes(j * 12)
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
                    var url = $"{_url}/erablieres/{i}/Baril";
                    var data = GenererBarils(i, j);

                    Console.WriteLine(url);

                    var response = _client.PostAsync(url, data).Result;

                    Console.WriteLine(response.StatusCode);
                    Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                }
            }
        }

        private static HttpContent GenererBarils(int idErabliere, int idbarils)
        {
            var barils = new Baril
            {
                IdÉrablière = idErabliere,
                DF = new DateTime(),
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
                var url = $"{_url}/erablieres";
                var data = NouvelleErabliere(i);

                Console.WriteLine(url);

                var response = _client.PostAsync(url, data).Result;

                Console.WriteLine(response.StatusCode);
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            }
        }

        private static HttpContent NouvelleErabliere(int i)
        {
            var erabliere = new Erablieres()
            {
                Nom = "Érablière {i}"
            };

            return erabliere.ToStringContent();
        }
    }

    public static class StringContextExtension
    {
        public static StringContent ToStringContent(this object obj)
        {
            var json = JsonConvert.SerializeObject(obj);

            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}
