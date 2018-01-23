using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReSharpersCooperation.Models;
using ReSharpersCooperation.Models.ProductVIewModels;
using System.Threading.Tasks;

namespace ReSharpersCooperation.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly OrdersRepository _ordersRepository;
        private readonly CartItemRepository _cartItemRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        //1
        public OrdersController(OrdersRepository ordersRepository,CartItemRepository cartItemRepo, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _ordersRepository = ordersRepository;
            _cartItemRepo = cartItemRepo;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public ViewResult Checkout()
        {
            return View(new Orders());
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(Orders order)
        {
            var user= await _userManager.GetUserAsync(User);
            if (ModelState.IsValid) 
            {
                var cart=_cartItemRepo.FindUserCart(user.UserName);
                order.UserName = user.UserName;
                _ordersRepository.SaveOrder(order);
                return RedirectToAction("Completed");
            }
            else
            {
                return View(order);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Completed()
        {
            var user = await _userManager.GetUserAsync(User);
            _cartItemRepo.Clear(user.UserName);
            return View();
        }
    }
}