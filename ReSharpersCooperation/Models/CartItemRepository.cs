using ReSharpersCooperation.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace ReSharpersCooperation.Models.ProductVIewModels
{
    public class CartItemRepository
    {
        private ApplicationDbContext db;
        public CartItemRepository(ApplicationDbContext db)
        {
            this.db = db;
        }
        public IQueryable<Cart_Item> Cart_Items => db.Cart_Item;

        public void AddToCart(int productNo, string username, int quantity)
        {
            var search = Cart_Items.SingleOrDefault(u => u.UserName == username && u.ProductNo == productNo);

            if (search == null)
            {
                db.Cart_Item.Add(new Cart_Item
                {
                    ProductNo = productNo,
                    Quantity = quantity,
                    UserName = username
                });
            }
            else
            {
                search.Quantity += quantity;
                if (search.Quantity == 0)
                {
                    db.Cart_Item.Remove(search);
                }
            }
            db.SaveChanges();

        }

        public List<Cart_Item> FindUserCart(string username)
        {
            var search = Cart_Items.Where(u => u.UserName == username).ToList();

            if (search == null)
            {
               return null; 
            }
            else
            {
                return search;
            }
        }

        public int CountCartProducts(string username)
        {
            return Cart_Items.Where(u => u.UserName == username).ToList().Count();

        }
    }
}
