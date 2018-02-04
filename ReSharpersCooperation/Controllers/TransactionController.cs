using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReSharpersCooperation.Models;

namespace ReSharpersCooperation.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly TransactionRepository _transactionRepository;

        public TransactionController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,TransactionRepository transactionRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _transactionRepository = transactionRepository;
        }
    
        public async Task<IActionResult> MyTransactions()
        {
            var user = await _userManager.GetUserAsync(User);
            var transactions = _transactionRepository.GetUsersTransactions(user.UserName);
            return View(transactions);
        }
    }
}