using System;
using ReSharpersCooperation.Data;
using System.Collections.Generic;
using System.Linq;

namespace ReSharpersCooperation.Models
{
    public class TotalOrdersRepository
    {
        private ApplicationDbContext db;

        public TotalOrdersRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IQueryable<TotalOrders> TotalOrders => db.TotalOrders;

        public void SaveOrder(Orders order, List<Cart_Item> cart)
        {
            foreach (var item in cart)
            {
                var temp = db.Product.Single(x => x.ProductNo == item.ProductNo);
                db.TotalOrders.Add(new TotalOrders
                {
                    OrderId = order.OrderId,
                    ProductNo = temp.ProductNo,
                    OrderDate = order.OrderDate,
                    ProductName = temp.ProductName,
                    Quantity = item.Quantity,
                    UserName = order.UserName
                });
            }
            db.SaveChanges();
        }

        public List<TotalOrders> ViewOrders(string username)
        {
            var myOrders = db.TotalOrders.Where(x => x.UserName == username).ToList();
            return myOrders;
        }
    }
}