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
        public string ProductImage { get; set; }
        public int ProductNo { get; set; }
        public int StockNo { get; set;}

        public CartSummaryViewModel(string productname, decimal cost , int quantity,string productimage,int productNo,int stockNo)
        {
            ProductName = productname;
            Cost = cost;
            Quantity = quantity;
            ProductImage = productimage;
            ProductNo = productNo;
            StockNo = stockNo;
        }
    }
}
