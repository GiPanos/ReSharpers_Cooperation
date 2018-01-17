using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReSharpersCooperation.Models.CartViewModels
{
    public class CartSummaryViewModel
    {
        public string ProductName { get; set; }
        public decimal Cost { get; set; }
        public int Quantity { get; set; }

        public CartSummaryViewModel(string productname, decimal cost , int quantity)
        {
            ProductName = productname;
            Cost = cost;
            Quantity = quantity;
        }
    }
}
