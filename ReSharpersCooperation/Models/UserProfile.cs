using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReSharpersCooperation.Models
{
    public class UserProfile
    {
        [Key]
        public int UserProfileId { get; set; }
        public string ImgPath { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }

        public UserProfile()
        {

        }
        public UserProfile(string imgPath,string name,string username)
        {
            Name = name;
            ImgPath = imgPath;
            UserName = username;
        }
    }
    
}
