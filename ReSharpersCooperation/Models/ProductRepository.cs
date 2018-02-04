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

        public void UpdateProduct(Product product,bool paymember=false)
        {
            if (paymember)
            {
                var user = db.Users.SingleOrDefault(u => u.UserName == product.UserName);
                user.Balance += product.Price / 3;
                var admin = db.Users.SingleOrDefault(u => u.UserName == "resharper@gmail.com");
                admin.Balance -= product.Price / 3;
            }
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
            prod.IsDeleted = true;
            prod.IsActive = false;
            if (prod != null)
            {
                db.Product.Update(prod);
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
                case "Rating":
                    var RatingFirstResult = db.Product.Where(p => p.ProductName.Contains(query) || p.ProductDesc.Contains(query)).OrderByDescending(p => p.Rating);
                    if (RatingFirstResult.Count() == 0)
                    {
                        RatingFirstResult = db.Product.OrderByDescending(p => p.Rating);
                    }
                    return RatingFirstResult;
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

        public List<Product> CheckStock(List<Cart_Item> cart)
        {
            var productlist = new List<Product>();
            foreach (var item in cart)
            {
                Product prod = db.Product.FirstOrDefault(p => p.ProductNo == item.ProductNo);
                if (item.Quantity > prod.StockNo)
                {
                    productlist.Add(prod);
                }
            }
            return productlist;
        }

        public void UpdateStock(List<Cart_Item> cart)
        {
            //var productlist = new List<Product>();
            foreach (var item in cart)
            {
                Product prod = db.Product.FirstOrDefault(p => p.ProductNo == item.ProductNo);
                prod.StockNo -= item.Quantity;
                UpdateProduct(prod);
            }

        }
        public List<Product> GetOfferedProducts (string username)
        {
            return db.Product.Where(u => u.UserName == username && !u.IsDeleted).ToList();
            
        }

    }
}
