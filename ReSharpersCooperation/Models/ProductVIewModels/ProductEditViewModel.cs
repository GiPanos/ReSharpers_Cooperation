﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
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
        public decimal Price { get; set; }
        public int StockNo { get; set; }
        public int Rating { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsFeatured { get; set; }

    }
}
