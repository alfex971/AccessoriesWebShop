using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.WebPages;
using AccessoriesWebShop.Models;

namespace AccessoriesWebShop.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
		}

	    [HttpPost]
	    public ActionResult Login(MongoUser user, string returnUrl)
	    {
		    if (isValidUser(user))
		    {
			    FormsAuthentication.SetAuthCookie(user.Email, false);
			    return Redirect(returnUrl);
		    }

		    return View(user);
	    }

	    private bool isValidUser(MongoUser user)
	    {
		    return (user.Email == "test@test.com" && user.Password == "test");
	    }

	    public ActionResult Logout()
	    {
		    FormsAuthentication.SignOut();
		    return Redirect("/Home/Index");
	    }
	}
}