using NotificationService_Micro.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NotificationService_Micro
{
    public class SubscriberService: BackgroundService
    {
        private readonly ILogger<SubscriberService> _logger;
        private readonly IConnectionMultiplexer _redis;

        public SubscriberService(ILogger<SubscriberService> logger)
        {
            _logger = logger;
            _redis = ConnectionMultiplexer.Connect("localhost:6379"); // Redis connection
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var subscriber = _redis.GetSubscriber();

            await subscriber.SubscribeAsync("orders_channel", (channel, message) =>
            {
                var order = JsonSerializer.Deserialize<Order>(message);
                _logger.LogInformation($"📦 New Order Received: {order.OrderId} - {order.CustomerName} ordered {order.Product} for ${order.Price}");

                // Simulate sending notification
                Console.WriteLine($"🔔 Notification Sent: {order.CustomerName}, your order for {order.Product} is being processed!");
            });

            await Task.Delay(-1, stoppingToken);
        }
    }

    public class Order
    {
        public string OrderId { get; set; }
        public string CustomerName { get; set; }
        public string Product { get; set; }
        public decimal Price { get; set; }
    }
    //private readonly IConnectionMultiplexer _redis;

    //public SubscriberService(IConnectionMultiplexer redis)
    //{
    //    _redis = redis;
    //}

    //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    //{
    //    var subscriber = _redis.GetSubscriber();
    //    await subscriber.SubscribeAsync("order_channel", (channel, message) =>
    //    {
    //        Console.WriteLine($"📢 New Order Received: {message}");
    //    });

    //    while (!stoppingToken.IsCancellationRequested)
    //    {
    //        await Task.Delay(1000, stoppingToken);
    //    }
    //}

}
