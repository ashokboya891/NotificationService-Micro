using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService_Micro.Models
{
    public class Order
    {
        public string OrderId { get; set; }
        public string Product { get; set; }
        public decimal Amount { get; set; }

    }
}
