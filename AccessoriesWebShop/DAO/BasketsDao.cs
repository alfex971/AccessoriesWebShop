using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using AccessoriesWebShop.Models;

namespace AccessoriesWebShop.DAO
{
	public class BasketsDao
	{
		private accessoriesEntities db = new accessoriesEntities();

		public Basket GetBasket(int? id)
		{
			var basket = db.Baskets.Find(id);
			return basket;
		}

		public Basket InsertBasket(Basket basket)
		{
			db.Baskets.Add(basket);
			db.SaveChanges();
			return basket;
		}

		public Basket UpdateBasket(Basket basket)
		{
			db.Entry(basket).State = EntityState.Modified;
			db.SaveChanges();
			return basket;
		}

        public Basket DeleteBasket(int id, string adName)
        {
            Basket basket = db.Baskets.Find(id, adName);
            db.Baskets.Remove(basket);
            db.SaveChanges();
            return basket;
        }
    }
}