using ReSharpersCooperation.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReSharpersCooperation.Models
{
    public class UserProfileRepository
    {
        private ApplicationDbContext db;

        public UserProfileRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void EditUserProfile(UserProfile userprofile)
        {
            var checkifprofileexists = db.UserProfile.SingleOrDefault(u => u.UserName == userprofile.UserName);
            if (checkifprofileexists == null)
            {
                db.UserProfile.Add(new UserProfile (userprofile.ImgPath,userprofile.Name,userprofile.UserName));
            }
            else
            {
                db.UserProfile.Remove(checkifprofileexists);
                db.SaveChanges();
                db.UserProfile.Add(userprofile);
                
            }
            
            db.SaveChanges();
        }
        public string  GetName(string username)
        {
            var name = db.UserProfile.SingleOrDefault(u => u.UserName == username);
            if (name == null)
            {
                return "";
            }
            else
            {
                return name.Name;
            }
            
        }
        public string GetProfilePic(string username)
        {
            var pic = db.UserProfile.SingleOrDefault(u => u.UserName == username);
            if ( pic == null)
            {
                return $@"\images\Profiles\NoProfilePic.jpg"; 
            }
            else
            {
                return pic.ImgPath;
            }
        }
    }
}
