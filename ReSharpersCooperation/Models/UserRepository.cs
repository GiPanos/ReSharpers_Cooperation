using Microsoft.AspNetCore.Identity;
using ReSharpersCooperation.Data;
using ReSharpersCooperation.Models.AccountViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReSharpersCooperation.Models
{
    public class UserRepository
    {

        private ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(ApplicationDbContext db, UserManager<ApplicationUser> usermanager)
        {
            this.db = db;
            _userManager = usermanager;
        }

        public void ApproveMember(string userid)
        {
            db.Users.SingleOrDefault(u => u.Id == userid).HasRequestedMember=false;
            db.SaveChanges();
            
        }
        public void RequestMembership(string userid)
        {
            db.Users.SingleOrDefault(u => u.Id == userid).HasRequestedMember = true;
            db.SaveChanges();
        }
    }
}
