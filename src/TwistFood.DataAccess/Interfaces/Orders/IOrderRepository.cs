﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwistFood.Domain.Entities.Order;

namespace TwistFood.DataAccess.Interfaces.Orders
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
    }
}
