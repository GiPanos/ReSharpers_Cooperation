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

        public ViewResult List(string category=null,int productPage = 1)
        {
            ViewBag.InCatalog = true;
            return View(new ProductListViewModel
            {
                Products = repository.Products
                            .Where(p=>p.ProductCategory==category || category==null)
                            .Skip((productPage - 1) * PageSize)
                            .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    TotalPages = (int)Math.Ceiling((double)repository.Products.Where(p=>p.ProductCategory==category|| category==null).Count() / (double)PageSize)
                },
                CurrentCategory=category,
            });
        }
        public ViewResult SearchResult(string query,string type)
        {
            ViewBag.InCatalog = true;
            var searchresults = repository.SearchProducts(query, type);
            return View("List", new ProductListViewModel
            {
                Products = searchresults,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = 1,
                    TotalPages = (int)Math.Ceiling((double)repository.Products.Where(p => p.ProductCategory == null || type== null).Count() / (double)PageSize)
                },
                CurrentCategory = null,
            });

        }

    }
}