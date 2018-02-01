using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReSharpersCooperation.Models;
using ReSharpersCooperation.Models.ProductVIewModels;
using System.Threading.Tasks;
using ReSharpersCooperation.Models.OrdersViewModel;

namespace ReSharpersCooperation.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly OrdersRepository _ordersRepository;
        private readonly TotalOrdersRepository _totalOrdersRepository;
        private readonly ProductRepository _productRepository;
        private readonly CartItemRepository _cartItemRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public OrdersController(TotalOrdersRepository totalOrdersRepository, OrdersRepository ordersRepository, ProductRepository productRepository, CartItemRepository cartItemRepo, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _totalOrdersRepository = totalOrdersRepository;
            _ordersRepository = ordersRepository;
            _productRepository = productRepository;
            _cartItemRepo = cartItemRepo;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public ViewResult Checkout()
        {
            return View(new OrdersViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(Orders order)
        {
            order.OrderStatusNo = 0;
            order.OrderDate = DateTime.Now;
            order.Shipped = false;

            var user = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                var cart = _cartItemRepo.FindUserCart(user.UserName);
                var outofstock = _productRepository.CheckStock(cart);

                if (outofstock.Count == 0)
                {
                    _productRepository.UpdateStock(cart);
                    order.UserName = user.UserName;
                    _ordersRepository.SaveOrder(order);
                    _totalOrdersRepository.SaveOrder(order, cart);
                    return RedirectToRoute("order");
                }
                else
                {
                    return View(new OrdersViewModel { OutOfStock = outofstock });
                }
            }
            else
            {
                return View(new OrdersViewModel());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Completed()
        {
            // if quantity<=stock:OK afairese apo Products to stock
            //else Minima lathous kai redirect  
            var user = await _userManager.GetUserAsync(User);
            _cartItemRepo.Clear(user.UserName);
            return View();
        }
    }
}