using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReSharpersCooperation.Models.UserViewModel
{
    public class UserViewModel
    {
        public ApplicationUser User { get; set; }
        public string Role { get; set; }
        public string Balance { get; set; }
    }
}
