using System;
using ReSharpersCooperation.Data;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace ReSharpersCooperation.Models
{
    public class TotalOrdersRepository
    {
        private ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;

        public TotalOrdersRepository(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            _userManager = userManager;
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
                    ProductName = temp.ProductName,
                    Quantity = item.Quantity,
                    OrderDate = order.OrderDate,
                    UserName = order.UserName
                });
            }
            db.SaveChanges();
        }
        public void ShareProfits(List<string> members,string admin,decimal totalcost)
        {
            decimal coefficient = 3M;
            decimal membershare = (totalcost  / coefficient)/ (members.Count());
            foreach (var user in db.Users)
            {
                if (members.Contains(user.UserName))
                {
                    user.Balance += membershare;
                }
                if (user.UserName == admin)
                {
                    user.Balance += (totalcost / coefficient);
                }
            }
            db.SaveChanges();
        }
        public void RemoveMoney(string username,decimal totalcost)
        {
            db.Users.SingleOrDefault(u => u.UserName == username).Balance -= totalcost;
            db.SaveChanges();
        }

        public List<TotalOrders> ViewOrders(string username)
        {
            var myOrders = db.TotalOrders.Where(x => x.UserName == username).ToList();
            return myOrders;
        }




    }
}
