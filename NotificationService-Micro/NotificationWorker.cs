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
    public class NotificationWorker:BackgroundService
    {
        private readonly IConnectionMultiplexer _redis;

        public NotificationWorker(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var subscriber = _redis.GetSubscriber();

            // Subscribe to Order Updates
            await subscriber.SubscribeAsync("order_channel", (channel, message) =>
            {
                var orderUpdate = JsonSerializer.Deserialize<OrderUpdate>(message);
                Console.WriteLine($"📩 Sending Email: Order {orderUpdate.OrderId} is now {orderUpdate.Status}");
            });

            // Subscribe to Payment Updates
            await subscriber.SubscribeAsync("payment_channel", (channel, message) =>
            {
                var paymentStatus = JsonSerializer.Deserialize<PaymentStatus>(message);
                Console.WriteLine($"📩 Sending Email: Payment for Order {paymentStatus.OrderId} is {paymentStatus.Status}");
            });

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

    }
}
