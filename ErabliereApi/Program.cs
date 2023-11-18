using System.Collections;
using ErabliereApi;
using ErabliereApi.StripeIntegration;

await Host.CreateDefaultBuilder(Environment.GetCommandLineArgs())
    .ConfigureLogging((hostBuildContext, builder) =>
    {
        builder.AddConsole();
    })
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<Startup>();
        webBuilder.ConfigureKestrel((c, o) =>
        {
            Console.WriteLine("ASPNETCORE_ENVIRONMENT: " + c.Configuration["ASPNETCORE_ENVIRONMENT"]);
            Console.WriteLine("ASPNETCORE_URLS: " + c.Configuration["ASPNETCORE_URLS"]);
            Console.WriteLine("ASPNETCORE_HTTP_PORT: " + c.Configuration["ASPNETCORE_HTTP_PORT"]);
            Console.WriteLine("ASPNETCORE_HTTPS_PORT: " + c.Configuration["ASPNETCORE_HTTPS_PORT"]);

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
    })
    .Build()
    .UseStripeUsageReccordTask()
    .RunAsync();