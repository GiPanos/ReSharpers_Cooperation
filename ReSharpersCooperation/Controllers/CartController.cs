using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReSharpersCooperation.Models;
using ReSharpersCooperation.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
//
namespace ReSharpersCooperation.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private ProductRepository repository;
        private Cart cart = new Cart();

        public CartController(ProductRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index()
        {
            ViewBag.ReturnUrl = TempData["returnUrl"];
            return View(cart);
        }

        public RedirectToRouteResult AddToCart(int ProductNo, string returnUrl)
        {
            Product product = repository.Products.SingleOrDefault(x => x.ProductNo == ProductNo);

            if (product != null)
            {
                cart.AddProduct(product, 1);
            }
            TempData["returnUrl"] = returnUrl;
            return RedirectToRoute("cart");
        }
    }
}
