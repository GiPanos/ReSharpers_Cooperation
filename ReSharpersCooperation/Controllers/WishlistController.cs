using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReSharpersCooperation.Models;
using ReSharpersCooperation.Models.WishlistViewModels;
using ReSharpersCooperation.Models.ProductVIewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReSharpersCooperation.Controllers
{
    [Authorize]
    public class WishlistController : Controller
    {
        private readonly ProductRepository _repository;
        private readonly WishlistItemRepository _wishlistitemrepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public WishlistController(ProductRepository repository, WishlistItemRepository wishlistitemrepo, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _repository = repository;
            _wishlistitemrepo = wishlistitemrepo;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var wishlist = _wishlistitemrepo.FindUserWishlist(user.UserName);

            List<WishlistSummaryViewModel> userwishlist = new List<WishlistSummaryViewModel>();

            int itemSum = 0;
            foreach (var item in wishlist)
            {
                foreach (var product in _repository.Products)
                {
                   
                    if (item.ProductNo == product.ProductNo)
                    {
                        userwishlist.Add(new WishlistSummaryViewModel(product.ProductName, product.Price, product.ProductImage, product.ProductNo, product.StockNo));
                        itemSum++;
                    }
                    
                }
            }

            ViewData["totalitems"] = itemSum;
            ViewBag.ReturnUrl = TempData["returnUrl"];
            return View(userwishlist);
        }

        public async Task<IActionResult> AddToWishlist (int productNo, string returnUrl)
        {
            var user = await _userManager.GetUserAsync(User);
            Product product = _repository.Products.SingleOrDefault(x => x.ProductNo == productNo);

            if (product != null)
            {
                _wishlistitemrepo.AddToWishlist(productNo, user.UserName);
            }

            TempData["returnUrl"] = returnUrl;           
            return RedirectToRoute("wishlist");
        }

        public async Task<IActionResult> RemoveItemFromWishlist(int productNo, string returnUrl)
        {
            var user = await _userManager.GetUserAsync(User);
            Product product = _repository.Products.SingleOrDefault(x => x.ProductNo == productNo);

            if (product != null)
            {
                _wishlistitemrepo.RemoveFromWishlist(productNo, user.UserName);
            }

            TempData["returnUrl"] = returnUrl;
            return RedirectToRoute("wishlist");
        }
    }
}