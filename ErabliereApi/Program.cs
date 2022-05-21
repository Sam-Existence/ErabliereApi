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
            c.Configuration.GetSection("Kestrel").Bind(o, co =>
            {
                co.ErrorOnUnknownConfiguration = c.Configuration.GetValue<bool?>("KestrelBinder.ErrorOnUnknowConfiguration") ?? true;
            });

            Console.WriteLine("Kestrel configuration:");
            Console.WriteLine($"AddServerHeader: {o.AddServerHeader}");
            Console.WriteLine($"Limits.MaxRequestHeaderCount: {o.Limits.MaxRequestHeaderCount}");
            Console.WriteLine($"Limits.MaxRequestBodySize: {o.Limits.MaxRequestBodySize}");
            Console.WriteLine($"Limits.MinRequestBodyDataRate.BytesPerSecond: {o.Limits.MinRequestBodyDataRate?.BytesPerSecond}");
        });
    }).Build().Run();