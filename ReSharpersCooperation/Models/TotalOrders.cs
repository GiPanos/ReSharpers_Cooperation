using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;

namespace ReSharpersCooperation.Models
{
    public class TotalOrders
    {
        public int OrderId { get; set; }
        public int ProductNo { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string UserName { get; set; }


        public TotalOrders()
        {
                
        }
    }
}
