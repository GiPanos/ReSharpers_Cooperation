﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReSharpersCooperation.Models;
using ReSharpersCooperation.Models.UserViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ReSharpersCooperation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ProductRepository _repository;
        private TotalOrdersRepository _totalOrdersRepository;
        private IHostingEnvironment _hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private TransactionRepository _transactionRepository;
        private OrdersRepository _ordersRepository;
        private UserRepository _userRepository;

        public AdminController(ProductRepository repository, TotalOrdersRepository totalOrdersRepository, 
            IHostingEnvironment hostingEnvironment,SignInManager<ApplicationUser> signinmanager,
            UserManager<ApplicationUser> usermanager,TransactionRepository transactionrepository,
            OrdersRepository ordersRepository,UserRepository userRepository)
        {
            _repository = repository;
            _totalOrdersRepository = totalOrdersRepository;
            _hostingEnvironment = hostingEnvironment;
            _userManager = usermanager;
            _signInManager = signinmanager;
            _transactionRepository = transactionrepository;
            _ordersRepository = ordersRepository;
            _userRepository = userRepository;
        }

        public ViewResult Index()
        {
            return View(_repository.Products.Where(p=>!p.IsDeleted));
        }

        public ViewResult AdminViewTotalOrders()
        {
            var myorders = _totalOrdersRepository.ViewOrdersAsAdmin().GroupBy(i => i.OrderId);
            return View(myorders);
        }

        public ViewResult Home()
        {
            return View();
        }

        public async Task<IActionResult> Ban(string userid)
        {
            var usertoban = await _userManager.FindByIdAsync(userid);
            await _userManager.SetLockoutEndDateAsync(usertoban, new DateTimeOffset(DateTime.Now.AddYears(10)));
            return RedirectToAction("Users");
        }

        public async Task<IActionResult> ApproveMember(string userid)
        {
            var usertoapprove = await _userManager.FindByIdAsync(userid);
            var roleresult=await _userManager.AddToRoleAsync(usertoapprove, "Member");
            var updateresult=await _userManager.UpdateAsync(usertoapprove);
            return RedirectToAction("Users");
        }
        public async Task<IActionResult> UnBan(string userid)
        {
            var usertounban = await _userManager.FindByIdAsync(userid);
            await _userManager.SetLockoutEndDateAsync(usertounban,null);
            return RedirectToAction("Users");
        }


        public async Task<IActionResult> Users()
        {
            var users = new List<UserViewModel>();
            foreach (var user in _userManager.Users)
            {

                if ( await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    users.Add(new UserViewModel { User = user, Balance = user.Balance.ToString("C2"), Role = "Admin" });
                }
                else if(await _userManager.IsInRoleAsync(user, "Member"))
                {
                    users.Add(new UserViewModel { User = user, Balance = user.Balance.ToString("C2"), Role = "Member" });
                }
                else
                {
                    users.Add(new UserViewModel { User = user, Balance = "N/A", Role = "Client" });
                };
            }
            var sortedusers = users.OrderBy(o => o.Role).ToList();
            var allusers = new AllUserViewModel { Users = sortedusers };
            return View(allusers);
        }
        

        public ViewResult Edit(int productNo)
        {
            var temp = _repository.Products.FirstOrDefault(p => p.ProductNo == productNo);
            ViewData["img"] = temp.ProductImage;

            return View(new ProductEditViewModel
            {
                ProductNo = temp.ProductNo,
                Price = temp.Price,
                IsActive = temp.IsActive,
                ProductDesc = temp.ProductDesc,
                ProductName = temp.ProductName,
                StockNo = temp.StockNo,
                Rating = temp.Rating,
                IsFeatured = temp.IsFeatured,
                CreatedDate = temp.CreatedDate,
                ProductCategory = temp.ProductCategory,
                ImageLink=temp.ProductImage,
                UserName=temp.UserName,
                isPaid=temp.isPaid,
                Longitude=temp.Longitude,
                Latitude=temp.Latitude,
                CatchType=temp.CatchType
            }
                );
        }
        [HttpPost]
        public IActionResult Edit(ProductEditViewModel product)
        {
            //Validation for Image size
            if (product.Image!=null && product.Image.Length > 1000000)
            {
                ViewData["ValidationError"] = "Image Size Exceeded 1 MB.Please upload another image";
                return View(product);
            }
            if (ModelState.IsValid)
            {
                
                //Assign View Model Values to Actual Product
                var newproduct = new Product
                {
                    
                    ProductNo = product.ProductNo,
                    ProductDesc = product.ProductDesc,
                    CreatedDate = product.CreatedDate,
                    ModifiedDate = DateTime.Now,
                    IsDeleted = product.IsDeleted,
                    IsActive = product.IsActive,
                    Rating = product.Rating,
                    Price = product.Price,
                    IsFeatured = product.IsFeatured,
                    ProductName = product.ProductName,
                    StockNo = product.StockNo,
                    ProductCategory = product.ProductCategory,
                    UserName=product.UserName,
                    Longitude=product.Longitude,
                    Latitude=product.Latitude,
                    CatchType=product.CatchType
                };
                if (product.FirstTime)
                {
                    newproduct.isPaid = true;
                    newproduct.IsActive = true;
                    
                }
                //The following code takes user image and stores it on wwwroot and the imagelink is being stored to database
                long size = 0;
                if (product.Image != null)
                {
                    var filename = ContentDispositionHeaderValue
                                .Parse(product.Image.ContentDisposition)
                                .FileName
                                .Trim('"');
                    var lastchars = filename.TakeLast(4);
                    string suffix = "";
                    foreach (var item in lastchars)
                    {
                        suffix += item;
                    }
                    filename = _hostingEnvironment.WebRootPath + $@"\\images\\Products\\{product.ProductName}{suffix}";
                    size += product.Image.Length;
                    using (FileStream fs = System.IO.File.Create(filename))
                    {
                        product.Image.CopyTo(fs);
                        fs.Flush();
                        fs.Close();
                    }
                    newproduct.ProductImage = $"\\images\\Products\\{product.ProductName}{suffix}";
                }
                //case user didn't insert new image keep the same image link
                else
                {
                    newproduct.ProductImage = product.ImageLink;
                }
                if (product.FirstTime)
                {
                    _repository.UpdateProduct(newproduct, true);
                    _transactionRepository.RegisterTransaction("resharper@gmail.com", newproduct.UserName, newproduct.Price / 3, "Admin Payment", newproduct.ProductNo);
                }
                else
                {
                    _repository.UpdateProduct(newproduct);
                }
                
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(product);
            }
        }

        public ViewResult Create()
        {

            return View(new ProductEditViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductEditViewModel product)
        {
            //case Image is too large
            if (product.Image.Length > 1000000)
            {
                ViewData["ValidationError"] = "Image Size Exceeded 1 MB.Please upload another image";
                return View(product);
            }
            if (ModelState.IsValid)
            {
                //folowing code takes image and stores it to wwwroot and stores image link to database
                long size = 0;
                var filename = ContentDispositionHeaderValue
                    .Parse(product.Image.ContentDisposition)
                    .FileName
                    .Trim('"');
                var lastchars = filename.TakeLast(4);
                string suffix = "";
                foreach (var item in lastchars)
                {
                    suffix += item;
                }
                filename = _hostingEnvironment.WebRootPath + $@"\images\Products\{product.ProductName}{suffix}";
                size += product.Image.Length;
                using (FileStream fs = System.IO.File.Create(filename))
                {
                    product.Image.CopyTo(fs);
                    fs.Flush();
                    fs.Close();
                }
                var user = await _userManager.GetUserAsync(User);
                //Create new product according to user input
                var newproduct = new Product
                {
                    ProductImage = $"\\images\\Products\\{product.ProductName}{suffix}",
                    ProductNo = product.ProductNo,
                    ProductDesc = product.ProductDesc,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    IsDeleted = false,
                    IsActive = product.IsActive,
                    Rating = product.Rating,
                    Price = product.Price,
                    IsFeatured = product.IsFeatured,
                    ProductName = product.ProductName,
                    StockNo = product.StockNo,
                    ProductCategory = product.ProductCategory,
                    UserName=user.UserName
                    

                };
                _repository.SaveProduct(newproduct);
                
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(product);
            }

        }

        [HttpPost]
        public IActionResult Delete(int productId)
        {
            var product = _repository.DeleteProduct(productId);

            if (product != null)
            {
                TempData["Message"] = $"{product.ProductName} was deleted.";
            }
            return RedirectToAction(nameof(Index));
        }

        public PartialViewResult Details(int productNo)
        {
            var product = _repository.Products.FirstOrDefault(p => p.ProductNo == productNo);
            return PartialView("_Details", product);
        }

        public IActionResult Transactions()
        {
            return View(_transactionRepository.GetAllTransactions());
        }
        public RedirectToActionResult ShipThisOrder(int OrderId)
        {
            _ordersRepository.ShipThisOrder(OrderId);
            return RedirectToAction(nameof(AdminViewTotalOrders));
        }

        public ViewResult Search(string year)
        {
            var myorders = _totalOrdersRepository.ViewOrdersAsAdmin().ToList();
            var searchresults = _totalOrdersRepository.SearchOrders(year, myorders).GroupBy(i => i.OrderId);
            return View("AdminViewTotalOrders", searchresults);
        }
    }
}