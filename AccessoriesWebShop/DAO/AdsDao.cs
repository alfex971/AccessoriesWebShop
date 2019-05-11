using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using AccessoriesWebShop.Models;

namespace AccessoriesWebShop.DAO
{
	public class AdsDao
	{
		private accessoriesEntities db = new accessoriesEntities();

		public List<Ad> GetAds()
		{
			var ads = db.Ads;
			return ads.ToList();
		}

		public Ad InsertAd(Ad ad)
		{
			db.Ads.Add(ad);
			db.SaveChanges();
			return ad;
		}

		public Ad UpdateAd(Ad ad)
		{
			db.Entry(ad).State = EntityState.Modified;
			db.SaveChanges();
			return ad;
		}

		public ObjectResult<Ad> SearchAdsByString(string searchCriteria)
		{
			return db.SP_Search(0, searchCriteria, null);
		}

		public ObjectResult<Ad> SearchAdsBySubCategory(short id)
		{
			return db.SP_Search(id, null, null);
		}

		public ObjectResult<Ad> Filter(short id, string searchCriteria, string filterCriteria)
		{
			return db.SP_Search(id, searchCriteria, filterCriteria);
		}

		public Ad DeleteAd(string id)
		{
			Ad ad = db.Ads.Find(id);
			db.Ads.Remove(ad);
			db.SaveChanges();
			return ad;
		}
	}
}