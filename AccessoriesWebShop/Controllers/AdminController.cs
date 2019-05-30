using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AccessoriesWebShop.DAO;
using AccessoriesWebShop.Models;
using AccessoriesWebShop.Utils;

namespace AccessoriesWebShop.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AdminController : Controller
	{
		private accessoriesEntities db = new accessoriesEntities();
		private AdsDao adsDao = new AdsDao();

		public AdminController()
		{
			db.ChangeDatabase(userId: "manager");
		}

		// GET: Admin
		public ActionResult Index()
		{
			return View("~/Views/Home/Index.cshtml");
		}

		[HttpPost]
		public ActionResult SearchByString(string search)
		{
			var ads = adsDao.SearchAdsByString(search);
			ViewBag.searched = search;
			ViewBag.categoryID = 0;
			ViewData["filterName"] = "price(low - high)";

			ViewBag.ItemsInBasket = 0;

			ViewBag.isAdmin = true;
			return View("~/Views/Ads/Index.cshtml", ads);
		}

		public ActionResult Sales()
		{
			var stats = db.Statistics;
			return View(stats);
		}

		// GET: Ads/Details/5
		public ActionResult AdsDetails(string id)
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
			return View("~/Views/Ads/Details.cshtml", ad);
		}

		// GET: Ads/Create
		public ActionResult AdsCreate()
		{
			ViewBag.categoryID = new SelectList(db.Categories, "id", "name");
			return View("~/Views/Ads/Create.cshtml");
		}

		// POST: Ads/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult AdsCreate(Ad ad)
		{
			if (ModelState.IsValid)
			{
				var insertedAd = db.Ads.Add(ad);
				db.SaveChanges();
				if (insertedAd != null)
				{
					return RedirectToAction("Index");
				}
			}

			ViewBag.categoryID = new SelectList(db.Categories, "id", "name", ad.categoryID);
			return View("~/Views/Ads/Create.cshtml", ad);
		}

		// GET: Ads/Edit/5
		public ActionResult AdsEdit(string id)
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
			return View("~/Views/Ads/Edit.cshtml", ad);
		}

		// POST: Ads/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult AdsEdit(Ad ad)
		{
			if (ModelState.IsValid)
			{
				db.Entry(ad).State = EntityState.Modified;
				db.SaveChanges();
				if (ad != null)
				{
					return RedirectToAction("Index");
				}
			}
			ViewBag.categoryID = new SelectList(db.Categories, "id", "name", ad.categoryID);
			return View("~/Views/Ads/Edit.cshtml", ad);
		}

		[HttpPost]
		public ActionResult Filter()
		{
			var selectedValue = Request.Form["Filter"];
			var ads = adsDao.Filter(short.Parse(selectedValue.Split(' ').Last(), CultureInfo.InvariantCulture), selectedValue.Split(' ')[1], selectedValue.Split(' ').First());
			ViewBag.searched = selectedValue.Split(' ')[1];
			ViewBag.categoryID = selectedValue.Split(' ').Last();
			ViewBag.isAdmin = true;

			return View("~/Views/Ads/Index.cshtml", ads);
		}

		public ActionResult SearchBySubCategory(short id)
		{
			var ads = adsDao.SearchAdsBySubCategory(id);
			ViewBag.searched = "";
			ViewBag.categoryID = id;
			ViewBag.isAdmin = true;

			return View("~/Views/Ads/Index.cshtml", ads);
		}

		// GET: Ads/Delete/5
		public ActionResult AdsDelete(string id)
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
			return View("~/Views/Ads/Delete.cshtml", ad);
		}

		// POST: Ads/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult AdsDeleteConfirmed(string id)
		{
			Ad ad = db.Ads.Find(id);
			db.Ads.Remove(ad);
			db.SaveChanges();
			return RedirectToAction("Index");
		}

	}
}