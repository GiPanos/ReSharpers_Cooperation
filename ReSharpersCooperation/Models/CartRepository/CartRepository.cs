﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReSharpersCooperation.Data;

namespace ReSharpersCooperation.Models.CartRepository
{
    public class CartRepository
    {
        private ApplicationDbContext db;

        public CartRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IQueryable<Cart> Carts=> db.Cart;
        private List<Cart_Item> items = new List<Cart_Item>();

        public virtual void AddProduct(int productNo, int quantity)
        {
            if (items.Count == 0)
            {
                var tempcart = new Cart();
                db.Cart.Add(tempcart);
                db.SaveChanges();
                Cart_Item item = items
                    .Where(x => x.ProductNo == productNo)
                    .SingleOrDefault();

                if (item == null)
                {
                    var newcartitem = new Cart_Item(productNo, quantity, tempcart.CartId);
                    db.Cart_Item.Add(newcartitem);
                    db.SaveChanges();
                }
                else
                {
                    item.Quantity += quantity;
                }
            }


            db.SaveChanges();
        }

        public virtual void Clear()
        {
            items.Clear();
        }

        //public decimal CalculateTotalCost()
        //{
        //    return items.Sum(x => x.Product.Price * x.Quantity);
        //}

    }
    }
