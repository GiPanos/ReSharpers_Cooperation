using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ReSharpersCooperation.Models
{
    public class Orders
    {
        [Key]
        public int OrderId { get; set; }

        public int OrderStatusNo { get; set; }
        public DateTime OrderDate { get; set; }


        [Required(ErrorMessage = "You have to provide a name!")]
        public string OrderName { get; set; }
        [Required(ErrorMessage = "You have to provide an address!")]
        public string Address { get; set; }
        [Required(ErrorMessage = "You have to provide a city!")]
        public string City { get; set; }
        [Required(ErrorMessage = "You have to provide a country!")]
        public string Country { get; set; }
        [Required(ErrorMessage = "You have to provide a zip code!")]
        public int Zip { get; set; }
        [BindNever]
        public bool Shipped { get; set; }
        public decimal TotalCost { get; set; }
        public string UserName { get; set; }


        public Orders()
        { }
    }
}
