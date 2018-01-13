using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReSharpersCooperation.Models;

namespace ReSharpersCooperation.Controllers
{
    public class ProductController : Controller
    {
        private ProductRepository repository;
        public int PageSize = 3;

        public ProductController(ProductRepository repo)
        {
            repository = repo;
        }

        public ViewResult List(int productPage = 1)
        {
            ViewBag.InCatalog = true;
            return View(new ProductListViewModel
            {
                Products = repository.Products
                            .Skip((productPage - 1) * PageSize)
                            .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    TotalPages = (int)Math.Ceiling((double)repository.Products.Count() / (double)PageSize)
                },
            });
        }

    }
}