using Microsoft.AspNetCore.Mvc;
using ReSharpersCooperation.Models;
using ReSharpersCooperation.Models.FiltersViewModel;
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
            ViewBag.CurrentCatchType = RouteData.Values["catchtype"];
            var categories = repository.GetAllCategories();
            var catchtypes = repository.GetAllCatchTypes();
            var filterslist = new FiltersViewModel { CatchTypes = catchtypes, Categories = categories };
            
            return View(filterslist);
        }
    }
}
