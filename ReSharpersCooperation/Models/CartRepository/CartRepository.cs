using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ReSharpersCooperation.Data;

namespace ReSharpersCooperation.Models.CartRepository
{
    public class CartRepository
    {
        private ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public CartRepository(ApplicationDbContext db, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            this.db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IQueryable<Cart> Carts => db.Cart;
        private List<Cart_Item> items = new List<Cart_Item>();

        public virtual int AddProduct(int productNo, string username, int quantity)
        {

            var usercart = db.Cart.SingleOrDefault(x => x.Username == username);
            if (usercart == null)
            {
                var newcart = new Cart(username);
                db.Cart.Add(newcart);
                db.SaveChanges();
                var newcartitem = new Cart_Item(productNo, quantity, newcart.CartId);
                db.Cart_Item.Add(newcartitem);
                db.SaveChanges();
                return newcart.CartId;
            }
            else
            {
                var usercartitem = db.Cart_Item.SingleOrDefault(p => p.ProductNo == productNo && p.CartId == usercart.CartId);
                if (usercartitem == null)
                {
                    var newcartitem = new Cart_Item(productNo, quantity, usercart.CartId);
                    db.Cart_Item.Add(newcartitem);
                    db.SaveChanges();
                }
                else
                {
                    usercartitem.Quantity += quantity;
                }
                db.SaveChanges();
                return usercart.CartId;
            }

        }

        public virtual void Clear()
        {
            items.Clear();
        }

        //public decimal CalculateTotalCost()
        //{
        //    return items.Sum(x => x.Product.Price * x.Quantity);
        //}

    }
}
