﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AccessoriesWebShop.Models;
using AccessoriesWebShop.Utils;

namespace AccessoriesWebShop.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CheckoutController : Controller
    {
        private accessoriesEntities db = new accessoriesEntities();

        // GET: Checkout
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Checkout()
		{
			db.ChangeDatabase(userId: "customer");
			var userID = db.Users.FirstOrDefault(x => x.email == User.Identity.Name).id;
            var checkedOut = db.Checkout(userID);
            return Redirect("/Home/Index");
        }
    }
}