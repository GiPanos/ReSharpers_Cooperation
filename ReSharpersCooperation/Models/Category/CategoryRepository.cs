using ReSharpersCooperation.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReSharpersCooperation.Models.Category
{
    public class CategoryRepository
    {
        private ApplicationDbContext db;

        public CategoryRepository(ApplicationDbContext db)
        {
            this.db = db;
        }
        public IQueryable<Category> Categories => db.Category;
        public IQueryable<Category_Product> CategoryProducts => db.Category_Product;
    }
}
