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
        private readonly TransactionRepository _transactionRepository;

        public OrdersController(TotalOrdersRepository totalOrdersRepository, OrdersRepository ordersRepository,
            ProductRepository productRepository, CartItemRepository cartItemRepo,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            TransactionRepository transactionRepository)
        {
            _totalOrdersRepository = totalOrdersRepository;
            _ordersRepository = ordersRepository;
            _productRepository = productRepository;
            _cartItemRepo = cartItemRepo;
            _userManager = userManager;
            _signInManager = signInManager;
            _transactionRepository = transactionRepository;
        }

        [HttpGet]
        public async Task<ViewResult> Checkout()
        {
            var user = await _userManager.GetUserAsync(User);
            decimal totalcost = _cartItemRepo.CalculateUserCartCost(user.UserName);
            return View(new OrdersViewModel {CurrentBalance = user.Balance, TotalCost = totalcost});
        }


        public async Task<ViewResult> ViewMyOrders()
        {
            var user = await _userManager.GetUserAsync(User);
            var myorders = _totalOrdersRepository.ViewMyOrders(user.UserName).GroupBy(i => i.OrderId);
            ViewData["Order"] = TempData["Order"];
            return View(myorders);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(OrdersViewModel order)
        {

            var user = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                var cart = _cartItemRepo.FindUserCart(user.UserName);
                if (cart.Count==0)
                {
                    ViewData["Error"] = "Your Cart is Empty";
                    return View(new OrdersViewModel
                    {
                        TotalCost = order.TotalCost,
                        CurrentBalance = order.CurrentBalance
                    });
                }
                var outofstock = _productRepository.CheckStock(cart);

                if (outofstock.Count == 0)
                {
                    if (order.PaymentMethod == "sitebalance" && user.Balance >= order.TotalCost ||
                        order.PaymentMethod == "creditcard")
                    {
                        if (order.PaymentMethod == "creditcard" && (order.ExpirationYear==18 && order.ExpirationMonth==1) ||order.Cvv.ToString().Count()!=3 || order.ExpirationMonth==0  || order.CreditCardType==null  )
                        {
                            ViewData["Error"] = "Invalid Credit Card Details";
                            return View(new OrdersViewModel
                            {
                                TotalCost = order.TotalCost,
                                CurrentBalance = order.CurrentBalance
                            });

                        }

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
                            Zip = order.Zip,
                            TotalCost = order.TotalCost
                        };
                        int neworderid = _ordersRepository.SaveOrder(neworder);

                        _totalOrdersRepository.SaveOrder(neworder, cart);
                        var members = await _userManager.GetUsersInRoleAsync("Member");
                        var membernames = members.Select(u => u.UserName).ToList();
                        _totalOrdersRepository.ShareProfits(membernames, "resharper@gmail.com", order.TotalCost);
                        _transactionRepository.RegisterTransaction(user.UserName, "resharper@gmail.com",
                            (order.TotalCost / 2), "Admin Order Share", null, neworderid);
                        foreach (var mem in members)
                        {
                            _transactionRepository.RegisterTransaction(user.UserName, mem.UserName,
                                (order.TotalCost / 2) / (members.Count()), "Member Order Share", null, neworderid);
                        }
                        if (order.PaymentMethod == "sitebalance")
                        {
                            _totalOrdersRepository.RemoveMoney(user.UserName, order.TotalCost);
                        }
                        TempData["Order"] = "Order has been Sucessfully Placed";
                        return RedirectToAction(nameof(Completed));
                    }
                    else
                    {
                        ViewData["Error"] = "Not Enough Money";
                        return View(new OrdersViewModel
                        {
                            TotalCost = order.TotalCost,
                            CurrentBalance = order.CurrentBalance
                        });
                    }
                    ;


                }
                else
                {
                    return View(new OrdersViewModel
                    {
                        OutOfStock = outofstock,
                        TotalCost = order.TotalCost,
                        CurrentBalance = order.CurrentBalance
                    });
                }
            }
            else
            {
                return View(new OrdersViewModel {TotalCost = order.TotalCost, CurrentBalance = order.CurrentBalance});
            }
        }

        [HttpGet]
        public async Task<IActionResult> Completed()
        {
            var user = await _userManager.GetUserAsync(User);
            _cartItemRepo.Clear(user.UserName);
            return RedirectToAction(nameof(ViewMyOrders));
        }

        public async Task<ViewResult> Search(string year)
        {
            var user = await _userManager.GetUserAsync(User);
            var myorders = _totalOrdersRepository.ViewMyOrders(user.UserName).ToList();
            var searchresults = _totalOrdersRepository.SearchOrders(year, myorders).GroupBy(i => i.OrderId);
            return View("ViewMyOrders", searchresults);
        }
    }
}