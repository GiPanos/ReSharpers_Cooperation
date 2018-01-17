using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReSharpersCooperation.Models
{
    public class Cart
    {
        //private List<CartItem> items = new List<CartItem>();

        [Key]
        public int CartId { get; set; }
        public int TotalOrderPrice { get; set; }

        //public List<CartItem> Items => items;

        public virtual void AddProduct(int productNo, int quantity)
        {
            ////CartItem item = items
            //    .Where(x => x.Product.ProductNo == productNo)
            //    .SingleOrDefault();

            //if (item == null)
            //{
            //    items.Add(new CartItem(productNo, quantity));
            //}
            //else
            //{
            //    item.Quantity += quantity;
            //}
        }

        public Cart()
        {
            //CartId = 1;
            TotalOrderPrice = 0;
        
        }

        //public virtual void Clear()
        //{
        //    items.Clear();
        //}

        //public decimal CalculateTotalCost()
        //{
        //    //return items.Sum(x => x.Product.Price * x.Quantity);
        //}
    }
}
