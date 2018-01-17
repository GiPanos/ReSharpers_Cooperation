using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReSharpersCooperation.Models.CartViewModels
{
    public class CartSummaryViewModel
    {
        public int Products { get; set; }
        public decimal Cost { get; set; }

        public CartSummaryViewModel(int products, decimal cost)
        {
            Products = products;
            Cost = cost;
        }
    }
}
