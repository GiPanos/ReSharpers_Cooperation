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

            switch (type)
            {
                case "Name":
                    var NameResult= db.Product.Where(p => p.ProductName.Contains(query)).ToList();
                    NameResult.AddRange(db.Product.Where(p => p.ProductDesc.Contains(query) && p.ProductName.Contains(query)==false));
                    return NameResult;
                case "Category":
                    return db.Product.Where(p => p.ProductCategory.Contains(query));
                case "HighestFirst":
                    var HighestFirstResult= db.Product.Where(p => p.ProductName.Contains(query) || p.ProductDesc.Contains(query)).OrderByDescending(p => p.Price);
                    if (HighestFirstResult.Count()==0)
                    {
                        HighestFirstResult = db.Product.OrderByDescending(p => p.Price);
                    }
                    return HighestFirstResult;
                case "LowestFirst":
                    var LowestFirstResult= db.Product.Where(p => p.ProductName.Contains(query) || p.ProductDesc.Contains(query)).OrderBy(p => p.Price);
                    if (LowestFirstResult.Count()==0)
                    {
                        LowestFirstResult = db.Product.OrderBy(p => p.Price);
                    }
                    return LowestFirstResult;
                default:
                    break;
            }
           
            return db.Product;

        }

    }
}
