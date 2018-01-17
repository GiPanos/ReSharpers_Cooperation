using ReSharpersCooperation.Data;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
