using NotificationService_Micro.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using NotificationService_Micro.Models;

namespace NotificationService_Micro
{
    public class InventoryService: BackgroundService
    {
        private readonly ILogger<InventoryService> _logger;
        private readonly IConnectionMultiplexer _redis;

        public InventoryService(ILogger<InventoryService> logger, IConnectionMultiplexer redis)
        {
            _logger = logger;
            _redis = redis;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var subscriber = _redis.GetSubscriber();
            await subscriber.SubscribeAsync("order_channel", async (channel, message) =>
            {
                var order = JsonSerializer.Deserialize<Order>(message);
                _logger.LogInformation($"📦 Order Received: {order.OrderId}, Product: {order.Product}, Amount: {order.Price}");
                await UpdateInventory(order);
            });
        }

        private async Task UpdateInventory(Order order)
        {
            _logger.LogInformation($"✅ Updating inventory for Product: {order.Product}");
            await Task.Delay(500); // Simulating database update
            _logger.LogInformation($"📉 Inventory updated for {order.Product}");
        }
        //private readonly IConnectionMultiplexer _redis;
        //private readonly ILogger<InventoryService> _logger;

        //public InventoryService(IConnectionMultiplexer redis, ILogger<InventoryService> logger)
        //{
        //    _redis = redis;
        //    _logger = logger;
        //}

        //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        //{
        //    var subscriber = _redis.GetSubscriber();
        //    await subscriber.SubscribeAsync("order_channel", async (channel, message) =>
        //    {
        //        var order = JsonSerializer.Deserialize<Order>(message);
        //        Console.WriteLine($"📦 Inventory Service: Order received - {order.Product}");
        //        await subscriber.PublishAsync("notification_channel", $"Order {order.OrderId} processed.");
        //    });
        //var subscriber = _redis.GetSubscriber();
        //await subscriber.SubscribeAsync("order_placed", (channel, message) =>
        //{
        //    var order = JsonSerializer.Deserialize<Models.Order>(message);
        //    _logger.LogInformation("Reducing stock for Product: {ProductId}", order.ProductId);
        //});
        //}
    }
}
