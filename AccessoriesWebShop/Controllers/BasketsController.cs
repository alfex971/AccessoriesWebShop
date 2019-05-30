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
using AccessoriesWebShop.Utils;

namespace AccessoriesWebShop.Controllers
{
    [Authorize(Roles = "Customer")]
    public class BasketsController : Controller
    {
        private accessoriesEntities db = new accessoriesEntities();
        private BasketsDao basketDao = new BasketsDao();

	    public BasketsController()
	    {
		    db.ChangeDatabase(userId: "customer");
	    }

		// GET: Baskets
		public ActionResult Index()
        {
            var baskets = db.Baskets.Include(b => b.Ad).Include(b => b.User);
            return View(baskets.ToList());
        }

        public ActionResult BasketsPerPerson()
        {
            var userID = db.Users.FirstOrDefault(x => x.email == User.Identity.Name).id;
            var baskets = db.Baskets.Where(x => x.userID == userID);
            return View("Index", baskets);
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
        public ActionResult Edit(int? userId, string adName)
        {
            if (userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Basket basket = db.Baskets.Find(userId, adName);
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
        public ActionResult Delete(int? userId, string adName)
        {
            if (userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Basket basket = db.Baskets.Find(userId, adName);
            if (basket == null)
            {
                return HttpNotFound();
            }
            return View(basket);
        }

        // POST: Baskets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int userId, string adName)
        {
            basketDao.DeleteBasket(userId, adName);
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
