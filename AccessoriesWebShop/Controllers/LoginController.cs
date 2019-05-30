using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
				return Redirect(returnUrl ?? "/Home/Index");
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
			byte[] salt;
			new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
			var pbkdf2 = new Rfc2898DeriveBytes(user.password, salt, 1000);
			byte[] hash = pbkdf2.GetBytes(20);
			byte[] hashBytes = new byte[36];
			Array.Copy(salt, 0, hashBytes, 0, 16);
			Array.Copy(hash, 0, hashBytes, 16, 20);
			string savedPasswordHash = Convert.ToBase64String(hashBytes);

			var mongoUser = new MongoUser()
			{
				Email = user.email,
				Password = savedPasswordHash
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
				roleID = 1,
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
				new FilterDefinitionBuilder<MongoUser>().Where(x => x.Email == user.Email);
			var fetchedUser = collection.Find<MongoUser>(filter).ToList();

			/* Fetch the stored value */
			string savedPasswordHash = fetchedUser.First().Password;
			/* Extract the bytes */
			byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
			/* Get the salt */
			byte[] salt = new byte[16];
			Array.Copy(hashBytes, 0, salt, 0, 16);
			/* Compute the hash on the password the user entered */
			var pbkdf2 = new Rfc2898DeriveBytes(user.Password, salt, 1000);
			byte[] hash = pbkdf2.GetBytes(20);
			/* Compare the results */
			for (int i = 0; i < 20; i++)
			{
				if (hashBytes[i + 16] != hash[i])
				{
					return false;
				}
			}

			return true;
		}

		public ActionResult Logout()
		{
			FormsAuthentication.SignOut();
			return Redirect("/Home/Index");
		}
	}
}