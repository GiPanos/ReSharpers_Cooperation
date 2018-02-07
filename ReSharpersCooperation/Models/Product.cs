using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReSharpersCooperation.Models
{
    public class Product
    {
        [Key]
        public int ProductNo { get; set; }
        
        public string ProductName { get; set; }
        public string ProductDesc { get; set; }
        public decimal Price { get; set; }
        public int StockNo { get; set; }
        public int Rating { get; set; }
        [UIHint("Bool")]
        public bool IsActive { get; set; }
        [UIHint("Bool")]
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ProductImage { get; set; }
        [UIHint("Bool")]
        public bool IsFeatured { get; set; }
        public string ProductCategory { get; set; }
        public string UserName { get; set; }
        [UIHint("Bool")]
        public bool isPaid { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string CatchType { get; set; }
        public string Location { get; set; }


    }
}
