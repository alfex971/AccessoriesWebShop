using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.WebPages;
using AccessoriesWebShop.DAO;
using AccessoriesWebShop.Models;
using MongoDB.Driver;

namespace AccessoriesWebShop.Controllers
{
	public class LoginController : Controller
	{
		private accessoriesEntities db = new accessoriesEntities();
		private UsersDao usersDao = new UsersDao();

		// GET: Login
		public ActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Login(MongoUser user, string returnUrl)
		{
			var isValidUser = ValidateUser(user);


			if (isValidUser)
			{
				FormsAuthentication.SetAuthCookie(user.Email, false);
				return Redirect(returnUrl);
			}

			return View(user);
		}
		public ActionResult Register()
		{
			ViewBag.roleID = new SelectList(db.Roles, "id", "name");
			return View("View");
		}

		[HttpPost]
		public ActionResult Register(WholeUser user, string returnUrl)
		{
			ViewBag.roleID = new SelectList(db.Roles, "id", "name");

			var mongoUser = new MongoUser()
			{
				Email = user.email,
				Password = user.password
			};

			var client = new MongoClient("mongodb+srv://alfex971:alfexalf971@alekscluster-mpsds.mongodb.net/test?retryWrites=true");
			var mongoDatabase = client.GetDatabase("Accessories");
			var collection = mongoDatabase.GetCollection<MongoUser>("User");
			collection.InsertOne(mongoUser);

			var sqlUser = new User()
			{
				email = user.email,
				name = user.name,
				phone = user.phone,
				roleID = user.roleID,
			};

			var savedUser = usersDao.InsertUser(sqlUser);

			FormsAuthentication.SetAuthCookie(user.email, false);
			return Redirect("/Home/Index");
		}

		private bool ValidateUser(MongoUser user)
		{
			var client = new MongoClient("mongodb+srv://alfex971:alfexalf971@alekscluster-mpsds.mongodb.net/test?retryWrites=true");
			var mongoDatabase = client.GetDatabase("Accessories");
			var collection = mongoDatabase.GetCollection<MongoUser>("User");
			var filter =
				new FilterDefinitionBuilder<MongoUser>().Where(x => x.Email == user.Email && x.Password == user.Password);

			var validUser = collection.Find<MongoUser>(filter).ToList();

			return validUser.Count == 1;
		}

		public ActionResult Logout()
		{
			FormsAuthentication.SignOut();
			return Redirect("/Home/Index");
		}
	}
}