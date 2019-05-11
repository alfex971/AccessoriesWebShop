using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AccessoriesWebShop.DAO;
using AccessoriesWebShop.Models;

namespace AccessoriesWebShop.Controllers
{
    public class BasketsController : Controller
    {
        private accessoriesEntities db = new accessoriesEntities();
	    private BasketsDao basketDao = new BasketsDao();

		// GET: Baskets
		public ActionResult Index()
        {
            var baskets = db.Baskets.Include(b => b.Ad).Include(b => b.User);
            return View(baskets.ToList());
        }

        // GET: Baskets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
	        Basket basket = basketDao.GetBasket(id);
            if (basket == null)
            {
                return HttpNotFound();
            }
            return View(basket);
        }

        // GET: Baskets/Create
        public ActionResult Create()
        {
            ViewBag.adName = new SelectList(db.Ads, "name", "description");
            ViewBag.userID = new SelectList(db.Users, "id", "name");
            return View();
        }

        // POST: Baskets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Basket basket)
        {
            if (ModelState.IsValid)
            {
	            var insertedBasket = basketDao.InsertBasket(basket);
	            if (insertedBasket != null)
	            {
		            return RedirectToAction("Index");
				}
			}

            ViewBag.adName = new SelectList(db.Ads, "name", "description", basket.adName);
            ViewBag.userID = new SelectList(db.Users, "id", "name", basket.userID);
            return View(basket);
        }

        // GET: Baskets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Basket basket = db.Baskets.Find(id);
            if (basket == null)
            {
                return HttpNotFound();
            }
            ViewBag.adName = new SelectList(db.Ads, "name", "description", basket.adName);
            ViewBag.userID = new SelectList(db.Users, "id", "name", basket.userID);
            return View(basket);
        }

        // POST: Baskets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,userID,adName,quantity")] Basket basket)
        {
            if (ModelState.IsValid)
			{
				var updatedBasket = basketDao.UpdateBasket(basket);
				if (updatedBasket != null)
				{
					return RedirectToAction("Index");
				}
            }
            ViewBag.adName = new SelectList(db.Ads, "name", "description", basket.adName);
            ViewBag.userID = new SelectList(db.Users, "id", "name", basket.userID);
            return View(basket);
        }

        // GET: Baskets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Basket basket = db.Baskets.Find(id);
            if (basket == null)
            {
                return HttpNotFound();
            }
            return View(basket);
        }

        // POST: Baskets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
		{
			basketDao.DeleteBasket(id);
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
