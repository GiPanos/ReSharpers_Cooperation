using System;
using ReSharpersCooperation.Data;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
            cart.ToList().ForEach(item => db.TotalOrders.Add(new TotalOrders{OrderId = order.OrderId, ProductNo = item.ProductNo, Quantity = item.Quantity, UserName = item.UserName}));
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

        public List<TotalOrdersViewModel.TotalOrdersViewModel> ViewMyOrders(string username)
        {
            var totalorders = db.TotalOrders.Where(x => x.UserName == username).ToList();
            var totalordersVm = new List<TotalOrdersViewModel.TotalOrdersViewModel>();

            foreach (var item in totalorders)
            {
                var product = db.Product.Single(x => x.ProductNo == item.ProductNo);
                var order = db.Orders.Single(x => x.OrderId == item.OrderId);
                totalordersVm.Add(new TotalOrdersViewModel.TotalOrdersViewModel(order.OrderDate, order.TotalCost, item.OrderId, product.ProductName, item.Quantity, order.Shipped, order.OrderName));
            }
            return totalordersVm;
        }

        public List<TotalOrdersViewModel.TotalOrdersViewModel> ViewOrdersAsAdmin()
        {
            var totalorders = db.TotalOrders.ToList();
            var totalordersVm = new List<TotalOrdersViewModel.TotalOrdersViewModel>();

            foreach (var item in totalorders)
            {
                var product = db.Product.Single(x => x.ProductNo == item.ProductNo);
                var order = db.Orders.Single(x => x.OrderId == item.OrderId);
                totalordersVm.Add(new TotalOrdersViewModel.TotalOrdersViewModel(order.OrderDate, order.TotalCost, item.OrderId, product.ProductName, item.Quantity, order.Shipped, order.OrderName));
            }
            return totalordersVm;
        }
    }
}
