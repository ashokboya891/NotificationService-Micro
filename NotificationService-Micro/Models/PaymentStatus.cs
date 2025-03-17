using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService_Micro.Models
{
    public class PaymentStatus
    {
        public string OrderId { get; set; }
        public string Status { get; set; }
    }
}
