using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReSharpersCooperation.Models.Category
{
    public class Category
    {
        [Key]
        public string CategoryName { get; set; }
        public string CategoryDesc { get; set; }
        public string CategoryImage { get; set; }
        public string CategoryParent { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

    }
}
