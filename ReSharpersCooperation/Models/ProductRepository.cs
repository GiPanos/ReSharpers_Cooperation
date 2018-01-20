using ReSharpersCooperation.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReSharpersCooperation.Models
{
    public class ProductRepository
    {
        private ApplicationDbContext db;

        public ProductRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IQueryable<Product> Products => db.Product;

        public void UpdateProduct(Product product)
        {
            
            db.Update(product);
            db.SaveChanges();
        }

        public void SaveProduct(Product product)
        {
            db.Product.Add(product);
            db.SaveChanges();
        }

        public Product DeleteProduct(int productNo)
        {
            Product prod = db.Product.FirstOrDefault(p => p.ProductNo == productNo);

            if (prod != null)
            {
                db.Product.Remove(prod);
                db.SaveChanges();
            }

            return prod;
        }
        public List<string> GetAllCategories()
        {
            return db.Product.Select(p => p.ProductCategory).Distinct().OrderBy(c => c).ToList();
        }
        public IEnumerable<Product> SearchProducts (string query,string type)
        {
            return db.Product.Where(p => p.ProductName.Contains(query) || p.ProductDesc.Contains(query));
        }

    }
}
