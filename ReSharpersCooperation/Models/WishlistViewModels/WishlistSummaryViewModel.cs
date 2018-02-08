using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReSharpersCooperation.Models.WishlistViewModels
{
    public class WishlistSummaryViewModel
    {
        public string ProductName { get; set; }
        public decimal Cost { get; set; }        
        public string ProductImage { get; set; }
        public int ProductNo { get; set; }
        public int StockNo { get; set; }

        public WishlistSummaryViewModel(string productname, decimal cost, string productimage, int productNo, int stockNo)
        {
            ProductName = productname;
            Cost = cost;
            ProductImage = productimage;
            ProductNo = productNo;
            StockNo = stockNo;
        }
    }
}
