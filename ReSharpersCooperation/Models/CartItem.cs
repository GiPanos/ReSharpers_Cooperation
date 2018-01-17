using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReSharpersCooperation.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }

        public CartItem(Product p, int q)
        {
            Product = p;
            Quantity = q;
        }

        public CartItem()
        {

        }
    }
}
