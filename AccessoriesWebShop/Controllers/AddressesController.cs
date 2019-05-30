using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AccessoriesWebShop.Models;
using AccessoriesWebShop.Utils;

namespace AccessoriesWebShop.Controllers
{
    [Authorize(Roles = "Customer")]
    public class AddressesController : Controller
    {
        private accessoriesEntities db = new accessoriesEntities();

		public AddressesController()
		{
			db.ChangeDatabase(userId: "customer");
		}

		// GET: Addresses
		public ActionResult Index()
        {
            var addresses = db.Addresses.Include(a => a.User);
            return View(addresses.ToList());
        }
        public ActionResult AddressesForUser()
        {
            var userID = db.Users.FirstOrDefault(x => x.email == User.Identity.Name).id;
            var addresses = db.Addresses.Where(x => x.userID == userID);
            return PartialView("Index", addresses.ToList());
        }
        // GET: Addresses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address address = db.Addresses.Find(id);
            if (address == null)
            {
                return HttpNotFound();
            }
            return View(address);
        }

        // GET: Addresses/Create
        public ActionResult Create()
        {
            var userID = db.Users.FirstOrDefault(x => x.email == User.Identity.Name).id;
            ViewBag.userID = new SelectList(db.Users, "id", "name", userID);
            return View();
        }

        // POST: Addresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,userID,country,postCode,city,street")] Address address)
        {
            if (ModelState.IsValid)
            {
                var userID = db.Users.FirstOrDefault(x => x.email == User.Identity.Name).id;
                address.userID = userID;
                db.Addresses.Add(address);
                db.SaveChanges();
                return RedirectToAction("Index", "Checkout");
            }

            ViewBag.userID = new SelectList(db.Users, "id", "name", address.userID);
            return View(address);
        }

        // GET: Addresses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address address = db.Addresses.Find(id);
            if (address == null)
            {
                return HttpNotFound();
            }
            ViewBag.userID = new SelectList(db.Users, "id", "name", address.userID);
            return View(address);
        }

        // POST: Addresses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,userID,country,postCode,city,street")] Address address)
        {
            if (ModelState.IsValid)
            {
                db.Entry(address).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Checkout");
            }
            ViewBag.userID = new SelectList(db.Users, "id", "name", address.userID);
            return View(address);
        }

        // GET: Addresses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address address = db.Addresses.Find(id);
            if (address == null)
            {
                return HttpNotFound();
            }
            return View(address);
        }

        // POST: Addresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Address address = db.Addresses.Find(id);
            db.Addresses.Remove(address);
            db.SaveChanges();
            return RedirectToAction("Index", "Checkout");
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