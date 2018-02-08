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
                            .Where(p =>  p.IsActive == true && p.IsDeleted == false )
                            .Skip((productPage - 1) * PageSize)
                            .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    TotalPages = (int)Math.Ceiling((double)repository.Products.Where(p=> p.IsActive == true && p.IsDeleted == false ).Count() / (double)PageSize)
                },
                CurrentLocation = null,
                CurrentCategory=null,
                ListType=null,
                Query=null
            });
        }

        public ViewResult ListCategory(string category, int productPage=1)
        {
            ViewBag.InCatalog = true;
            return View("List", new ProductListViewModel
            {
                Products = repository.Products
                            .Where(p => p.ProductCategory == category && p.IsActive == true && p.IsDeleted == false)
                            .Skip((productPage - 1) * PageSize)
                            .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    TotalPages = (int)Math.Ceiling((double)repository.Products.Where(p => p.ProductCategory == category && p.IsActive == true && p.IsDeleted == false).Count() / (double)PageSize)
                },
                CurrentCategory = category,
                CurrentLocation = null,
                ListType = null,
                Query = null
            });
        }

        public ViewResult ListLocation(string location, int productPage = 1)
        {
            ViewBag.InCatalog = true;
            return View("List",new ProductListViewModel
            {
                Products = repository.Products
                            .Where(p => p.Location == location && p.IsActive == true && p.IsDeleted == false)
                            .Skip((productPage - 1) * PageSize)
                            .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    TotalPages = (int)Math.Ceiling((double)repository.Products.Where(p => p.Location == location && p.IsActive == true && p.IsDeleted == false).Count() / (double)PageSize)
                },
                CurrentLocation = location,
                ListType = null,
                Query = null
            });
        }
        public ViewResult ListCatchType(string catchtype, int productPage = 1)
        {
            ViewBag.InCatalog = true;
            return View("List", new ProductListViewModel
            {
                Products = repository.Products
                            .Where(p => p.CatchType ==catchtype && p.IsActive == true && p.IsDeleted == false)
                            .Skip((productPage - 1) * PageSize)
                            .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    TotalPages = (int)Math.Ceiling((double)repository.Products.Where(p => p.CatchType==catchtype && p.IsActive == true && p.IsDeleted == false).Count() / (double)PageSize)
                },
                CurrentLocation = null,
                ListType = null,
                Query = null,
                CurrentCatchType=catchtype
                
            });
        }
        public ViewResult Search(string query,string type,int productPage=1)
        {
            
            ViewBag.InCatalog = true;
            var searchresults = repository.SearchProducts(query, type);
            return View("List", new ProductListViewModel
            {
                Products = searchresults
                .Skip((productPage-1)*PageSize)
                .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    TotalPages = (int)Math.Ceiling((double)searchresults.Count() / (double)PageSize)
                },
                CurrentCategory = null,
                ListType=type,
                Query=query
                
            });
            

        }

    }
}