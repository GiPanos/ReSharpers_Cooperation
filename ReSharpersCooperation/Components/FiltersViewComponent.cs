using Microsoft.AspNetCore.Mvc;
using ReSharpersCooperation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReSharpersCooperation.Components
{
    public class FiltersViewComponent : ViewComponent
    {
        private ProductRepository repository;

        public FiltersViewComponent(ProductRepository repo)
        {
            repository = repo;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.CurrentCategory = RouteData.Values["category"];
            return View(repository.GetAllCategories());
        }
    }
}
