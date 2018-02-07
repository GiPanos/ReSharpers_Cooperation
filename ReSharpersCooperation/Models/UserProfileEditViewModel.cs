using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReSharpersCooperation.Models
{
    public class UserProfileEditViewModel
    {
        public IFormFile ProfilePic { get; set; }
        public string ImgPath { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
    }
}
