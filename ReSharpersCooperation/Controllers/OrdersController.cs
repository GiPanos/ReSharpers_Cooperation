using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReSharpersCooperation.Models;
using ReSharpersCooperation.Models.ProductVIewModels;
using System.Threading.Tasks;
using ReSharpersCooperation.Models.OrdersViewModel;
using System.Linq;

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

        public OrdersController(TotalOrdersRepository totalOrdersRepository, OrdersRepository ordersRepository,
            ProductRepository productRepository, CartItemRepository cartItemRepo, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _totalOrdersRepository = totalOrdersRepository;
            _ordersRepository = ordersRepository;
            _productRepository = productRepository;
            _cartItemRepo = cartItemRepo;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<ViewResult> Checkout()
        {
            var user = await _userManager.GetUserAsync(User);
            decimal totalcost = _cartItemRepo.CalculateUserCartCost(user.UserName);
            return View(new OrdersViewModel {CurrentBalance = user.Balance, TotalCost = totalcost});
        }

        public async Task<ViewResult> ViewOrders()
        {
            var user = await _userManager.GetUserAsync(User);
            var totalOrders = _totalOrdersRepository.ViewOrders(user.UserName).GroupBy(i => i.OrderId); 
            return View(totalOrders);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(OrdersViewModel order)
        {

            var user = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                var cart = _cartItemRepo.FindUserCart(user.UserName);
                var outofstock = _productRepository.CheckStock(cart);

                if (outofstock.Count == 0)
                {
                    if (user.Balance >= order.TotalCost)
                    {
                        
                        _productRepository.UpdateStock(cart);
                        var neworder = new Orders
                        {
                            City = order.City,
                            Country = order.Country,
                            Address = order.Address,
                            OrderDate = DateTime.Now,
                            OrderName = order.OrderName,
                            OrderStatusNo = 0,
                            Shipped = false,
                            UserName = user.UserName,
                            Zip = order.Zip

                        };
                        _ordersRepository.SaveOrder(neworder);
                        
                         _totalOrdersRepository.SaveOrder(neworder, cart);
                        var members = await _userManager.GetUsersInRoleAsync("Member");
                        var membernames = members.Select(u => u.UserName).ToList();
                        _totalOrdersRepository.ShareProfits(membernames,"resharper@gmail.com",order.TotalCost);
                        _totalOrdersRepository.RemoveMoney(user.UserName, order.TotalCost);
                        return RedirectToRoute("order");
                    }
                    else
                    {
                        ViewData["Error"] = "Not Enough Money";
                        return View(new OrdersViewModel { });
                    };

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
            var user = await _userManager.GetUserAsync(User);
            _cartItemRepo.Clear(user.UserName);
            return View();
        }
    }
}