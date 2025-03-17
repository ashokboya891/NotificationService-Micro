using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NotificationService_Micro;
using StackExchange.Redis;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(hostContext.Configuration["Redis:ConnectionString"]));
        services.AddHostedService<SubscriberService>();
        services.AddHostedService<NotificationWorker>();
        services.AddHostedService<InventoryService>();
    })
    .Build();

host.Run();
