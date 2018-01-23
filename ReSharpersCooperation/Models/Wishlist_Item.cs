using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ReSharpersCooperation.Models
{
    public class Wishlist_Item
    {
        [Key]
        public int WishlistItemId { get; set; }

        public int ProductNo { get; set; }
        public string Username { get; set; }

        public Wishlist_Item (int productNo, string username)
        {
            ProductNo = productNo;
            Username = username;
        }

        public Wishlist_Item()
        {

        }


    }
}
