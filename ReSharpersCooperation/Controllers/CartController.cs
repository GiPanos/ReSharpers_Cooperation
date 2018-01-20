using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReSharpersCooperation.Models;
using ReSharpersCooperation.Models.CartViewModels;
using ReSharpersCooperation.Models.ProductVIewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace ReSharpersCooperation.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ProductRepository repository;
        private readonly CartItemRepository _cartItemRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;


        public CartController(ProductRepository repo,CartItemRepository cartItemRepo, UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
            repository = repo;
            _cartItemRepo = cartItemRepo;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            var user =  await _userManager.GetUserAsync(User);

            var cart = _cartItemRepo.FindUserCart(user.UserName);
            decimal totalprice = 0M;
            List<CartSummaryViewModel> usercart = new List<CartSummaryViewModel>();
            //repository.Products.Where(p => p.ProductName == cart.Where(c=>c.ProductNo==p.ProductNo)));
            foreach (var item in cart)
            {
                foreach (var item2 in repository.Products )
                {
                    if (item2.ProductNo == item.ProductNo)
                    {
                        usercart.Add(new CartSummaryViewModel(item2.ProductName, item2.Price, item.Quantity));
                        totalprice += item2.Price * item.Quantity;
                    }
                }
            }
            ViewData["totalprice"] = totalprice;
            ViewBag.ReturnUrl = TempData["returnUrl"];
            return View(usercart);
        }

        public async Task<IActionResult> AddToCart(int ProductNo, string returnUrl)
        {
            var user = await _userManager.GetUserAsync(User);
            Product product = repository.Products.SingleOrDefault(x => x.ProductNo == ProductNo);
            if (product != null)
            {

                _cartItemRepo.AddToCart(ProductNo,user.UserName ,1);
            }
            TempData["returnUrl"] = returnUrl;
            return RedirectToRoute("cart");
            
        }
    }
}
