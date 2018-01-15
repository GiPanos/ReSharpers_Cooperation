using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReSharpersCooperation.Models;
using System.Drawing;

namespace ReSharpersCooperation.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private ProductRepository _repository;
        public AdminController(ProductRepository repository)
        {
            _repository = repository;
        }

        public ViewResult Index()
        {

            //var result = User.IsInRole("SuperAdmin");
            return View(_repository.Products);
        }

        public ViewResult Edit(int productNo)
        {
            var temp = _repository.Products.FirstOrDefault(p => p.ProductNo == productNo);
            //MemoryStream ms = new MemoryStream(temp.ProductImage);
            //var productviewmodel = new ProductEditViewModel { Product = temp, };
            return View();
        }

        [HttpPost]
        public IActionResult Edit(ProductEditViewModel product)
        {
            if (ModelState.IsValid)
            {
                IFormFile uploadedimage = product.Image;
                MemoryStream ms = new MemoryStream();
                uploadedimage.OpenReadStream().CopyTo(ms);
                //Image image = Image.FromStream(ms);
                //product.Product.ProductImage = ms.ToArray();
                //_repository.UpdateProduct(product.Product);
                //TempData["Message"] = $"{product.Product.ProductName} has been updated.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(product);
            }
        }

        public ViewResult Create()
        {
            //var lastproduct = _repository.Products.Last();
            //var tempproduct = new Product { ProductNo = lastproduct.ProductNo + 1 };

            return View(new ProductEditViewModel ());
        }

        [HttpPost]
        public IActionResult Create(ProductEditViewModel product)
        {
            if (ModelState.IsValid)
            {
                IFormFile uploadedimage = product.Image;
                MemoryStream ms = new MemoryStream();
                uploadedimage.OpenReadStream().CopyTo(ms);
                //Image image = Image.FromStream(ms);
                var newproduct = new Product { ProductImage = ms.ToArray(),
                    ProductNo = product.ProductNo,
                    ProductDesc = product.ProductDesc,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    IsDeleted = false,
                    IsActive = false,
                    Rating=product.Rating,
                    Price=product.Price,
                    IsFeatured=product.IsFeatured,
                    ProductName=product.ProductName,
                    StockNo=product.StockNo
 
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
