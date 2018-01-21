using System.Collections.Generic;

namespace ReSharpersCooperation.Models
{
    public class ProductListViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentCategory { get; set; }
        public string ListType { get; set; }
        public string Query { get; set; }
    }
    
}