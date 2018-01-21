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

namespace ReSharpersCooperation.Controllers
{
    [Authorize]
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

            if (User.IsInRole("Admin"))
            {
                return View(_repository.Products);
            }
            else
            {
                return View("AccessDenied");
            }
            
        }



        public ViewResult Edit(int productNo)
        {
            var temp = _repository.Products.FirstOrDefault(p => p.ProductNo == productNo);
            ViewData["img"] = temp.ProductImage;

            return View(new ProductEditViewModel
            {
                ProductNo = temp.ProductNo,
                Price = temp.Price,
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
            if (ModelState.IsValid)
            {

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
            if (product.Image.Length > 1000)
            {
                ViewData["ValidationError"] = "Image Size Exceeded 1 MB.Please upload another image";
                return View(product);
            }
            if (ModelState.IsValid)
            {
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
