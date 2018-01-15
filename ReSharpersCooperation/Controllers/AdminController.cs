using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReSharpersCooperation.Models;

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
            return View(temp);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _repository.UpdateProduct(product);
                TempData["Message"] = $"{product.ProductName} has been updated.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(product);
            }
        }

        public ViewResult Create()
        {
            return View(new Product());
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _repository.SaveProduct(product);
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
