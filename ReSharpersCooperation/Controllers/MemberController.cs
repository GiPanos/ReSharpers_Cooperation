using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReSharpersCooperation.Models;

namespace ReSharpersCooperation.Controllers
{
    [Authorize(Roles = "Member")]
    public class MemberController : Controller
    {

        private ProductRepository _repository;
        private IHostingEnvironment _hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public MemberController(ProductRepository repository, IHostingEnvironment hostingEnvironment, SignInManager<ApplicationUser> signinmanager, UserManager<ApplicationUser> usermanager)
        {
            _repository = repository;
            _hostingEnvironment = hostingEnvironment;
            _userManager = usermanager;
            _signInManager = signinmanager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult OfferNewProduct()
        {
            return View(new ProductEditViewModel { });
        }

        [HttpPost]
        public IActionResult OfferNewProduct(ProductEditViewModel product)
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
                    Price = product.Price*3,
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
    }
}