using AccessoriesWebShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccessoriesWebShop.RoleProvider
{
	public class SiteRole:System.Web.Security.RoleProvider
	{
		public override bool IsUserInRole(string username, string roleName)
		{
			throw new NotImplementedException();
		}

		public override string[] GetRolesForUser(string username)
		{
			accessoriesEntities db = new accessoriesEntities();
			var roleId = db.Users.FirstOrDefault(u => u.email == username).roleID;
			var role = db.Roles.FirstOrDefault(r => r.id == roleId).name;

			return new[] { role };
		}

		public override void CreateRole(string roleName)
		{
			throw new NotImplementedException();
		}

		public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
		{
			throw new NotImplementedException();
		}

		public override bool RoleExists(string roleName)
		{
			throw new NotImplementedException();
		}

		public override void AddUsersToRoles(string[] usernames, string[] roleNames)
		{
			throw new NotImplementedException();
		}

		public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
		{
			throw new NotImplementedException();
		}

		public override string[] GetUsersInRole(string roleName)
		{
			throw new NotImplementedException();
		}

		public override string[] GetAllRoles()
		{
			throw new NotImplementedException();
		}

		public override string[] FindUsersInRole(string roleName, string usernameToMatch)
		{
			throw new NotImplementedException();
		}

		public override string ApplicationName { get; set; }
	}
}