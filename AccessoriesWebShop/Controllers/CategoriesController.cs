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
    public class CategoriesController : Controller
    {
        private accessoriesEntities db = new accessoriesEntities();
	    private CategoriesDao categoriesDao = new CategoriesDao();

		// GET: Categories
		public ActionResult Index()
        {
            var categories = db.Categories.Include(c => c.Category2);
            return View(categories.ToList());
        }

	    public ActionResult MainCategories()
	    {
		    var categories = categoriesDao.GetMainCategories();
		    return PartialView("CategoriesNavBar", categories);
	    }

	    public ActionResult SubCategories(short? id)
	    {
		    if (id == null)
		    {
			    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		    }
			var categories = categoriesDao.GetSubCategoriesForMainCategory((short)id);
		    return PartialView("SubCategories", categories);
	    }

		// GET: Categories/Details/5
		public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            ViewBag.parentCategoryID = new SelectList(db.Categories, "id", "name");
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,parentCategoryID")] Category category)
        {
            if (ModelState.IsValid)
            {
	            categoriesDao.InsertcCategory(category);

				return RedirectToAction("Index");
            }

            ViewBag.parentCategoryID = new SelectList(db.Categories, "id", "name", category.parentCategoryID);
            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            ViewBag.parentCategoryID = new SelectList(db.Categories, "id", "name", category.parentCategoryID);
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,parentCategoryID")] Category category)
        {
            if (ModelState.IsValid)
            {
	            categoriesDao.UpdateCategory(category);
                return RedirectToAction("Index");
            }
            ViewBag.parentCategoryID = new SelectList(db.Categories, "id", "name", category.parentCategoryID);
            return View(category);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
	        categoriesDao.DeletecaCategory(id);

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
