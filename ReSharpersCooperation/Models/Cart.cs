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
        [Key]
        public int CartId { get; set; }
        public int TotalOrderPrice { get; set; }
        public string Username { get; set; }

        public Cart(string username)
        {
            TotalOrderPrice = 0;
            Username = username;

        }

        public Cart()
        {
            
        }
    }
}
