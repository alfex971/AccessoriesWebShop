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

		public Basket DeleteBasket(int id)
		{
			Basket basket = db.Baskets.Find(id);
			db.Baskets.Remove(basket);
			db.SaveChanges();
			return basket;
		}

        public int getNumAdsBaskets(short? userID = 1)
        {
            var baskets = db.Baskets.Where(b => b.userID == userID);
            int retVal = 0;

            foreach (Basket b in baskets)
            {
                retVal += b.quantity;
            }

            return retVal;
        }

        public int getNextIdNum()
        {
            int lastBasketNum = db.Baskets.LastOrDefault().id;
            return lastBasketNum + 1;
        }
    }
}