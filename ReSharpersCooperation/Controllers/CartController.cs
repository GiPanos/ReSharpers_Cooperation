using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReSharpersCooperation.Models;
using ReSharpersCooperation.Models.CartRepository;
using ReSharpersCooperation.Models.CartViewModels;
using ReSharpersCooperation.Models.ProductVIewModels;
using ReSharpersCooperation.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
//
namespace ReSharpersCooperation.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private ProductRepository repository;
        private CartRepository _cartrepo;
        private Cart cart = new Cart();
        private CartItemRepository _cartItemRepo;


        public CartController(ProductRepository repo, CartRepository cartrepo,CartItemRepository cartItemRepo)
        {
            repository = repo;
            _cartrepo = cartrepo;
            _cartItemRepo = cartItemRepo;
        }

        public ViewResult Index( int cartid)
        {
            //var usercart = _cartrepo.Carts.SingleOrDefault(x => x.CartId == cartid);
            var usercart = _cartrepo.Carts.SingleOrDefault(x => x.CartId == cartid);
            var usercartitems = _cartItemRepo.Cart_Items.Where(x => x.CartId == cartid);
            
            var result = new List<CartSummaryViewModel>();
            foreach (var item in usercartitems)
            {
                
                foreach (var p in repository.Products)
                {
                    if (item.ProductNo == p.ProductNo)
                    {
                        var tempcartsummaryviewmodel = new CartSummaryViewModel(p.ProductName, p.Price,item.Quantity);
                        result.Add(tempcartsummaryviewmodel);
                    }
                }
                
            }
            
            ViewBag.ReturnUrl = TempData["returnUrl"];
            return View(result);
        }

        public RedirectToRouteResult AddToCart(int ProductNo, string returnUrl)
        {
            Product product = repository.Products.SingleOrDefault(x => x.ProductNo == ProductNo);
            int cartidx = 0;
            if (product != null)
            {

                cartidx=_cartrepo.AddProduct(ProductNo, 1);
            }
            TempData["returnUrl"] = returnUrl;
            return RedirectToRoute(new
            {
                controller = "Cart",
                action = "Index",
                cartid = cartidx
            });
            
        }
    }
}
