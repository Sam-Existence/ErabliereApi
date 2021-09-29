using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace ErabliereApi
{
    /// <summary>
    /// Classe Program, contient le point d'entré du programme.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Fonction main. le point d'entré du programme.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            Console.WriteLine(Environment.ProcessorCount);
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Création de l'application
        /// </summary>
        /// <param name="args">Les arguments reçu de la ligne de commande</param>
        /// <returns>Une nouvelle instance de <see cref="IHostBuilder"/></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((hostBuildContext, builder) =>
                {
                    builder.AddConsole();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
