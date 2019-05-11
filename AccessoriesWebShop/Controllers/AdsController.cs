using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AccessoriesWebShop.Models;
using AccessoriesWebShop.DAO;

namespace AccessoriesWebShop.Controllers
{
	public class AdsController : Controller
	{
		private accessoriesEntities db = new accessoriesEntities();
		private AdsDao adsDao = new AdsDao();

		// GET: Ads
		public ActionResult Index()
		{
			var ads = adsDao.GetAds();
			ViewBag.searched = "";
			ViewBag.categoryID = 0;
			ViewData["filterName"] = "price(low - high)";

			return View(ads);
		}

		[HttpPost]
		public ActionResult SearchByString(string search)
		{
			var ads = adsDao.SearchAdsByString(search);
			ViewBag.searched = search;
			ViewBag.categoryID = 0;
			ViewData["filterName"] = "price(low - high)";

			return View("Index", ads);
		}

		public ActionResult SearchBySubCategory(short id)
		{
			var ads = adsDao.SearchAdsBySubCategory(id);
			ViewBag.searched = "";
			ViewBag.categoryID = id;
			ViewData["filterName"] = "price(low - high)";

			return View("Index", ads);
		}

		[HttpPost]
		public ActionResult Filter()
		{
			var selectedValue = Request.Form["Filter"];
			var ads = adsDao.Filter(short.Parse(selectedValue.Split(' ').Last(), CultureInfo.InvariantCulture), selectedValue.Split(' ')[1], selectedValue.Split(' ').First());
			ViewBag.searched = selectedValue.Split(' ')[1];
			ViewBag.categoryID = selectedValue.Split(' ').Last();

			return View("Index", ads);
		}

		// GET: Ads/Details/5
		public ActionResult Details(string id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Ad ad = db.Ads.Find(id);
			if (ad == null)
			{
				return HttpNotFound();
			}
			return View(ad);
		}

		// GET: Ads/Create
		public ActionResult Create()
		{
			ViewBag.categoryID = new SelectList(db.Categories, "id", "name");
			return View();
		}

		// POST: Ads/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Ad ad)
		{
			if (ModelState.IsValid)
			{
				var insertedAd = adsDao.InsertAd(ad);
				if (insertedAd != null)
				{
					return RedirectToAction("Index");
				}
			}

			ViewBag.categoryID = new SelectList(db.Categories, "id", "name", ad.categoryID);
			return View(ad);
		}

		// GET: Ads/Edit/5
		public ActionResult Edit(string id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Ad ad = db.Ads.Find(id);
			if (ad == null)
			{
				return HttpNotFound();
			}
			ViewBag.categoryID = new SelectList(db.Categories, "id", "name", ad.categoryID);
			return View(ad);
		}

		// POST: Ads/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(Ad ad)
		{
			if (ModelState.IsValid)
			{
				var updatedAd = adsDao.UpdateAd(ad);
				if (updatedAd != null)
				{
					return RedirectToAction("Index");
				}
			}
			ViewBag.categoryID = new SelectList(db.Categories, "id", "name", ad.categoryID);
			return View(ad);
		}

		// GET: Ads/Delete/5
		public ActionResult Delete(string id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Ad ad = db.Ads.Find(id);
			if (ad == null)
			{
				return HttpNotFound();
			}
			return View(ad);
		}

		// POST: Ads/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(string id)
		{
			adsDao.DeleteAd(id);
			return RedirectToAction("Index");
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
