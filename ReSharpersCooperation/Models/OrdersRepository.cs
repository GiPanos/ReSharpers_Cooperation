using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReSharpersCooperation.Data;
using StackExchange.Redis;

namespace ReSharpersCooperation.Models
{
    public class OrdersRepository
    {
        private ApplicationDbContext db;
        public OrdersRepository(ApplicationDbContext db)
        {
            this.db = db;
        }


        public int SaveOrder(Orders order)
        {
            db.Orders.Add(order);
            db.SaveChanges();
            return order.OrderId;
        }

        public void ShipThisOrder(int orderid)
        {
            db.Orders.SingleOrDefault(o => o.OrderId == orderid).Shipped = true;
            db.SaveChanges();

        }



    }
}
