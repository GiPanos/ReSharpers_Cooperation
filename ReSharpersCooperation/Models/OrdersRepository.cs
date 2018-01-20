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

        public void SaveOrder(Orders order)
        {
            //db.AddRange(order); Παίζει και με το AddRange
            db.Orders.Add(order);
            db.SaveChanges();
        }
        
    }
}
