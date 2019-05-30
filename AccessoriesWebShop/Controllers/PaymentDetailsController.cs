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
    public class PaymentDetailsController : Controller
    {
        private accessoriesEntities db = new accessoriesEntities();

	    public PaymentDetailsController()
	    {
		    db.ChangeDatabase(userId: "customer");
	    }

		// GET: PaymentDetails
		public ActionResult Index()
        {
            var paymentDetails = db.PaymentDetails.Include(p => p.User);
            return View(paymentDetails.ToList());
        }

        public ActionResult PaymentDetailsForUser()
        {
            var userID = db.Users.FirstOrDefault(x => x.email == User.Identity.Name).id;
            var paymentDetails = db.PaymentDetails.Where(x => x.userID == userID);
            return PartialView("Index", paymentDetails.ToList());
        }

        // GET: PaymentDetails/Details/5
        public ActionResult Details(long? cardNumber)
        {
            if (cardNumber == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentDetail paymentDetail = db.PaymentDetails.Find(cardNumber);
            if (paymentDetail == null)
            {
                return HttpNotFound();
            }
            return View(paymentDetail);
        }

        // GET: PaymentDetails/Create
        public ActionResult Create()
        {
            ViewBag.userID = new SelectList(db.Users, "id", "name");
            return View();
        }

        // POST: PaymentDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "userID,cardNumber,cvr,expiryDate")] PaymentDetail paymentDetail)
        {
            if (ModelState.IsValid)
            {
                var userID = db.Users.FirstOrDefault(x => x.email == User.Identity.Name).id;
                paymentDetail.userID = userID;
                db.PaymentDetails.Add(paymentDetail);
                db.SaveChanges();
                return RedirectToAction("Index", "Checkout");
            }

            ViewBag.userID = new SelectList(db.Users, "id", "name", paymentDetail.userID);
            return View(paymentDetail);
        }

        // GET: PaymentDetails/Edit/5
        public ActionResult Edit(long? cardNumber)
        {
            if (cardNumber == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentDetail paymentDetail = db.PaymentDetails.Find(cardNumber);
            if (paymentDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.userID = new SelectList(db.Users, "id", "name", paymentDetail.userID);
            return View(paymentDetail);
        }

        // POST: PaymentDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userID,cardNumber,cvr,expiryDate")] PaymentDetail paymentDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paymentDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Checkout");
            }
            ViewBag.userID = new SelectList(db.Users, "id", "name", paymentDetail.userID);
            return View(paymentDetail);
        }

        // GET: PaymentDetails/Delete/5
        public ActionResult Delete(long? cardNumber)
        {
            if (cardNumber == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentDetail paymentDetail = db.PaymentDetails.Find(cardNumber);
            if (paymentDetail == null)
            {
                return HttpNotFound();
            }
            return View(paymentDetail);
        }

        // POST: PaymentDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long? cardNumber)
        {
            PaymentDetail paymentDetail = db.PaymentDetails.Find(cardNumber);
            db.PaymentDetails.Remove(paymentDetail);
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