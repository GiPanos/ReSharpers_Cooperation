using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ReSharpersCooperation.Models
{
    public class Cart_Item
    {
        [Key]
        public int CartItemId { get; set; }
        
        public int ProductNo { get; set; }
       
        public int CartId { get; set; }
        public int Quantity { get; set; }
        public Cart_Item(int productNo, int q, int cartID)
        {
            ProductNo = productNo;
            Quantity = q;
            CartId = cartID;
        }

        public Cart_Item()
        { }
    }
}
