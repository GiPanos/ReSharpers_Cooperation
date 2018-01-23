using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReSharpersCooperation.Models
{
    public class Wishlist
    {
        private List<Wishlist_Item> items = new List<Wishlist_Item>();
        public List<Wishlist_Item> Items => items;

        public virtual void AddProduct()
        {

        }

    }
}
