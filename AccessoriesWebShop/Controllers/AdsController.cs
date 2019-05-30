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
using AccessoriesWebShop.Utils;
using MongoDB.Driver.Core.Configuration;
using accessoriesEntities = AccessoriesWebShop.Models.accessoriesEntities;

namespace AccessoriesWebShop.Controllers
{
	public class AdsController : Controller
	{
		private const string connectionString =
				@"metadata = res://*/Models.AccessoriesModel.csdl|res://*/Models.AccessoriesModel.ssdl|res://*/Models.AccessoriesModel.msl;provider=System.Data.SqlClient;provider connection string='data source=myprojects.database.windows.net;initial catalog=accessories;user id=nik;password=diliana12345!;MultipleActiveResultSets=True;App=EntityFramework'";
		private accessoriesEntities db = new accessoriesEntities();
		private AdsDao adsDao = new AdsDao();
		private BasketsDao basketsDao = new BasketsDao();

		// GET: Ads
		public ActionResult Index()
		{
			var ads = adsDao.GetAds();
			ViewBag.searched = "";
			ViewBag.categoryID = 0;
			ViewBag.categoryName = "";
			ViewData["filterName"] = "price(low - high)";


			var user = db.Users.FirstOrDefault(x => x.email == User.Identity.Name);

			if (user != null)
			{

				if (User.IsInRole("Admin"))
				{
					db.ChangeDatabase(userId: "manager", password: "pass");
				}
				else if (User.IsInRole("Customer"))
				{
					db.ChangeDatabase(userId: "customer", password: "pass");
				}

				var baskets = db.Baskets.Where(x => x.userID == user.id);
				var itemsInBasket = 0;

				foreach (var basket1 in baskets)
				{
					itemsInBasket += basket1.quantity;
				}

				ViewBag.ItemsInBasket = itemsInBasket;
			}

			return View(ads);
		}

		[AjaxAuthorize(Roles = "Customer")]
		[HttpPost]
		public JsonResult AddToBasket(string adName, int quantity)
		{
			db.ChangeDatabase(null, null, "customer", "pass");
			var userID = db.Users.FirstOrDefault(x => x.email == User.Identity.Name).id;
			var basket = db.Baskets.Where(x => x.adName == adName && x.userID == userID).ToList();
			if (basket.Count() != 0)
			{
				basket.First().quantity += quantity;

				db.Entry(basket.First()).State = EntityState.Modified;
				db.SaveChanges();
			}
			else
			{
				var newBasket = new Basket()
				{
					adName = adName,
					userID = userID,
					quantity = quantity,
				};

				basketsDao.InsertBasket(newBasket);
			}

			return Json(quantity);
		}

		[HttpPost]
		public ActionResult SearchByString(string search)
		{
			var ads = adsDao.SearchAdsByString(search);
			ViewBag.searched = search;
			ViewBag.categoryID = 0;
			ViewBag.categoryName = "";
			ViewData["filterName"] = "price(low - high)";

			var user = db.Users.FirstOrDefault(x => x.email == User.Identity.Name);

			if (user != null)
			{
				if (User.IsInRole("Admin"))
				{
					db.ChangeDatabase(userId: "manager", password: "pass");
				}
				else if (User.IsInRole("Customer"))
				{
					db.ChangeDatabase(userId: "customer", password: "pass");
				}

				var baskets = db.Baskets.Where(x => x.userID == user.id);
				var itemsInBasket = 0;

				foreach (var basket1 in baskets)
				{
					itemsInBasket += basket1.quantity;
				}

				ViewBag.ItemsInBasket = itemsInBasket;
			}

			return View("Index", ads);
		}

		public ActionResult SearchBySubCategory(short id)
		{
			var ads = adsDao.SearchAdsBySubCategory(id);
			ViewBag.searched = "";
			var chosenCateory = db.Categories.First(x => x.id == id);
			ViewBag.categoryID = id;
			ViewBag.categoryName = chosenCateory.name;
			ViewData["filterName"] = "price(low - high)";

			var user = db.Users.FirstOrDefault(x => x.email == User.Identity.Name);

			if (user != null)
			{
				if (User.IsInRole("Admin"))
				{
					db.ChangeDatabase(userId: "manager", password: "pass");
				}
				else if (User.IsInRole("Customer"))
				{
					db.ChangeDatabase(userId: "customer", password: "pass");
				}

				var baskets = db.Baskets.Where(x => x.userID == user.id);
				var itemsInBasket = 0;

				foreach (var basket1 in baskets)
				{
					itemsInBasket += basket1.quantity;
				}

				ViewBag.ItemsInBasket = itemsInBasket;
			}

			return View("Index", ads);
		}

		public ActionResult MostDiscounted()
		{
			var ads = db.MostDiscounteds;

			return PartialView(ads);
		}

		[HttpPost]
		public ActionResult Filter()
		{
			var selectedValue = Request.Form["Filter"];
			var ads = adsDao.Filter(short.Parse(selectedValue.Split(' ').Last(), CultureInfo.InvariantCulture), selectedValue.Split(' ')[1], selectedValue.Split(' ').First());
			ViewBag.searched = selectedValue.Split(' ')[1];

			if (short.Parse(selectedValue.Split(' ').Last()) != 0)
			{
				var categoryID = short.Parse(selectedValue.Split(' ').Last());
				var chosenCategory = db.Categories.First(x => x.id == categoryID);
				ViewBag.categoryID = selectedValue.Split(' ').Last();
				ViewBag.categoryName = chosenCategory.name;
			}
			else
			{
				ViewBag.categoryID = selectedValue.Split(' ').Last();
			}

			var user = db.Users.FirstOrDefault(x => x.email == User.Identity.Name);

			if (user != null)
			{
				if (User.IsInRole("Admin"))
				{
					db.ChangeDatabase(userId: "manager", password: "pass");
				}
				else if (User.IsInRole("Customer"))
				{
					db.ChangeDatabase(userId: "customer", password: "pass");
				}

				var baskets = db.Baskets.Where(x => x.userID == user.id);
				var itemsInBasket = 0;

				foreach (var basket1 in baskets)
				{
					itemsInBasket += basket1.quantity;
				}

				ViewBag.ItemsInBasket = itemsInBasket;
			}
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