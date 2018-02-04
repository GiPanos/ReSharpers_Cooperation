﻿using Microsoft.AspNetCore.Authorization;
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
            foreach (var c in cart)
            {
                foreach (var p in repository.Products )
                {
                    if (p.ProductNo == c.ProductNo)
                    {
                        usercart.Add(new CartSummaryViewModel(p.ProductName, p.Price, c.Quantity,p.ProductImage,p.ProductNo,p.StockNo));
                        totalprice += p.Price * c.Quantity;
                    }
                }
            }
            ViewData["totalprice"] = totalprice;
            ViewBag.ReturnUrl = TempData["returnUrl"];
            return View(usercart);
        }

        public async Task<IActionResult> AddToCart(int ProductNo, string returnUrl,int quantity=1)
        {
            //get username
            var user = await _userManager.GetUserAsync(User);
            //verify product exists
            Product product = repository.Products.SingleOrDefault(x => x.ProductNo == ProductNo);
            if (product != null)
            {
                //Function that adds item to cart
                _cartItemRepo.AddToCart(ProductNo,user.UserName ,quantity);
            }
            TempData["returnUrl"] = returnUrl;
            //View the cart
            return RedirectToRoute("cart");            
        }
        public async Task<IActionResult> RemoveFromCart(int ProductNo,string returnUrl,int quantity=1)
        {
            //get username
            var user = await _userManager.GetUserAsync(User);
            //verify product exists
            Product product = repository.Products.SingleOrDefault(x => x.ProductNo == ProductNo);
            if (product != null)
            {
                //Function that adds item to cart with quantity -1
                _cartItemRepo.AddToCart(ProductNo, user.UserName, -quantity);
            }
            TempData["returnUrl"] = returnUrl;

            return RedirectToRoute("cart");
        }
    }
}
