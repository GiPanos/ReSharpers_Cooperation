using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReSharpersCooperation.Models
{
    public class ProductEditViewModel
    {
        public IFormFile Image { get; set; }
        public int ProductNo { get; set; }
        public string ProductName { get; set; }
        public string ProductDesc { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Please enter a positive price")]
        public decimal Price { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a positive stock number.")]
        public int StockNo { get; set; }
        public int Rating { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsFeatured { get; set; }
        public string ProductCategory { get; set; }
        public string ImageLink { get; set; }
        public string UserName { get; set; }
        public bool FirstTime { get; set; }
        public bool isPaid { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string CatchType { get; set; }


    }
}
