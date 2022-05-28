using System.Collections;
using ErabliereApi;

Host.CreateDefaultBuilder(Environment.GetCommandLineArgs())
    .ConfigureLogging((hostBuildContext, builder) =>
    {
        builder.AddConsole();
    })
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<Startup>();
        webBuilder.ConfigureKestrel((c, o) =>
        {
            Console.WriteLine(c.Configuration["ASPNETCORE_ENVIRONMENT"]);

            try 
            {
                const string kestrelBinderOption = "KestrelBinder_ErrorOnUnknowConfiguration";

                var binderThrowOnError = c.Configuration.GetValue<bool?>(kestrelBinderOption);

                Console.WriteLine($"{kestrelBinderOption}: {(binderThrowOnError == null ? "null" : binderThrowOnError)}");
                if (binderThrowOnError == null)
                {
                    Console.WriteLine($"When {kestrelBinderOption} is null the default value is true");
                }

                c.Configuration.GetSection("Kestrel").Bind(o, co =>
                {
                    co.ErrorOnUnknownConfiguration = binderThrowOnError ?? true;
                });

                Console.WriteLine("Kestrel configuration:");
                Console.WriteLine($"AddServerHeader: {o.AddServerHeader}");
                Console.WriteLine($"Limits.MaxRequestHeaderCount: {o.Limits.MaxRequestHeaderCount}");
                Console.WriteLine($"Limits.MaxRequestBodySize: {o.Limits.MaxRequestBodySize}");
                Console.WriteLine($"Limits.MinRequestBodyDataRate.BytesPerSecond: {o.Limits.MinRequestBodyDataRate?.BytesPerSecond}");
            }
            catch
            {
                Console.Error.WriteLine("On error occure when configuring Kestrel");
                Console.Error.WriteLine("It may occure because of a missong environment variable. Here is the list of environment variables:");

                foreach (var env in Environment.GetEnvironmentVariables().Cast<DictionaryEntry>())
                {
                    Console.Error.WriteLine($"{env.Key}: {env.Value}");
                }

                throw;
            }
        });
    }).Build().Run();