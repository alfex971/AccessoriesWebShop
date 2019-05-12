using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AccessoriesWebShop.Models;

namespace AccessoriesWebShop.DAO
{
	public class UsersDao
	{
		private accessoriesEntities db = new accessoriesEntities();

		public User InsertUser(User user)
		{
			db.Users.Add(user);
			db.SaveChanges();
			return user;
		}

	}
}