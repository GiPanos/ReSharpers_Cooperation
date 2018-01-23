using ReSharpersCooperation.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;


namespace ReSharpersCooperation.Models
{
    public class WishlistItemRepository
    {
        private ApplicationDbContext db;
        public WishlistItemRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IQueryable<Wishlist_Item> WishlistItems => db.Wishlist_Item;

        public void AddToWishlist(int productNo, string username)
        {
            var item = WishlistItems.SingleOrDefault(u => u.Username == username && u.ProductNo == productNo);

            if (item == null)
            {
                db.Wishlist_Item.Add(new Wishlist_Item(productNo, username));
            }
            else
            {
                //Item already exist in the wishlist notification
                //should be added here
            }
            db.SaveChanges();
        }

        public void RemoveFromWishlist(int productNo, string username)
        {
            var item = WishlistItems.SingleOrDefault(u => u.Username == username && u.ProductNo == productNo);

            if (item != null)
            {
                db.Wishlist_Item.Remove(item);
            }
        }

        public List<Wishlist_Item> FindUserWishlist(string username)
        {
            var itemList = WishlistItems.Where(u => u.Username == username).ToList();

            if (itemList == null)
            {
                return null;
            }
            else
            {
                return itemList;
            }
        }

        public int CountWishlistProducts(string username)
        {
            int itemcount = WishlistItems.Where(u => u.Username == username).ToList().Count();
            return itemcount;
        }

        public virtual void Clear(string username)
        {
            var userWishlist = FindUserWishlist(username);
            foreach (var item in userWishlist)
            {
                db.Wishlist_Item.Remove(item);
            }
            db.SaveChanges();
        }
    }
}
