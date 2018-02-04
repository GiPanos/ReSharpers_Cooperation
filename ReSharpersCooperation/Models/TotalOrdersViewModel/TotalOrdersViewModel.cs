using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReSharpersCooperation.Models.TotalOrdersViewModel
{
    public class TotalOrdersViewModel
    {

        public DateTime OrderDate { get; set; }
        public decimal Total { get; set; }
        [Key]
        public int OrderId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public bool Shipped { get; set; }
        public string Username { get; set; }

        public TotalOrdersViewModel(DateTime orderDate, decimal total, int orderId, string productName, int quantity, bool shipped, string username)
        {
            OrderDate = orderDate;
            Total = total;
            OrderId = orderId;
            ProductName = productName;
            Quantity = quantity;
            Shipped = shipped;
            Username = username;
        }
    }
}
