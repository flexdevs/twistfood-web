using OnlineMarket.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwistFood.Service.ViewModels.Orders
{
    public class OrderDetailForAdminViewModel:BaseEntity
    {
        public long ProductId { get; set; } 
        public string ProductImagePath { get; set; }   = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public int Amount  { get; set; }
        public double Price { get; set; }   

    }
}
