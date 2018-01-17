using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReSharpersCooperation.Models
{
    public class Cart
    {
        private List<CartItem> items = new List<CartItem>();

        public List<CartItem> Items => items;

        public virtual void AddProduct(Product product, int quantity)
        {
            CartItem item = items
                .Where(x => x.Product.ProductNo == product.ProductNo)
                .SingleOrDefault();

            if (item == null)
            {
                items.Add(new CartItem(product, quantity));
            }
            else
            {
                item.Quantity += quantity;
            }
        }

        public virtual void Clear()
        {
            items.Clear();
        }

        public decimal CalculateTotalCost()
        {
            return items.Sum(x => x.Product.Price * x.Quantity);
        }
    }
}
