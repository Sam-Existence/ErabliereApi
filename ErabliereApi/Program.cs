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
            c.Configuration.Bind("Kestrel", o);

            Console.WriteLine("Kestrel configuration:");
            Console.WriteLine($"AddServerHeader: {o.AddServerHeader}");
            Console.WriteLine($"Limits.MaxRequestHeaderCount: {o.Limits.MaxRequestHeaderCount}");
            Console.WriteLine($"Limits.MaxRequestBodySize: {o.Limits.MaxRequestBodySize}");
        });
    }).Build().Run();