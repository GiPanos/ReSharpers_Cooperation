﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReSharpersCooperation.Models;
using System.Drawing;
using Microsoft.AspNetCore.Hosting.Internal;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using ReSharpersCooperation.Models.UserViewModel;

namespace ReSharpersCooperation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ProductRepository _repository;
        private IHostingEnvironment _hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AdminController(ProductRepository repository, IHostingEnvironment hostingEnvironment,SignInManager<ApplicationUser> signinmanager, UserManager<ApplicationUser> usermanager)
        {
            _repository = repository;
            _hostingEnvironment = hostingEnvironment;
            _userManager = usermanager;
            _signInManager = signinmanager;
        }

        public ViewResult Index()
        {
            return View(_repository.Products);
        }

        public ViewResult Home()
        {
            return View();
        }

        public async Task<IActionResult> Users()
        {
            var users = new List<UserViewModel>();
            foreach (var user in _userManager.Users)
            {

                if ( await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    users.Add(new UserViewModel { User = user, Balance = 0, Role = "Admin" });
                }
                else
                {
                    users.Add(new UserViewModel { User = user, Balance = 0, Role = "Client" });
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
                ImageLink=temp.ProductImage
                

            }
                );
        }
        [HttpPost]
        public IActionResult Edit(ProductEditViewModel product)
        {
            //Validation for Image size
            if (product.Image!=null && product.Image.Length > 1000)
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
                    ProductCategory = product.ProductCategory


                };
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

                _repository.UpdateProduct(newproduct);
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
        public IActionResult Create(ProductEditViewModel product)
        {
            //case Image is too large
            if (product.Image.Length > 1000)
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
                    ProductCategory = product.ProductCategory

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
    }
}
