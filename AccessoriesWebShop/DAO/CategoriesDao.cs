using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using AccessoriesWebShop.Models;

namespace AccessoriesWebShop.DAO
{
	public class CategoriesDao
	{
		private accessoriesEntities db = new accessoriesEntities();

		public List<Category> GetMainCategories()
		{
			var mainCategories = db.Categories.Where(c => c.parentCategoryID == null);
			return mainCategories.ToList();
		}

		public List<Category> GetSubCategoriesForMainCategory(short id)
		{
			var subCategories = db.Categories.Where(c => c.parentCategoryID == id);
			return subCategories.ToList();
		}
		public Category InsertcCategory(Category category)
		{
			db.Categories.Add(category);
			db.SaveChanges();
			return category;
		}


		public Category UpdateCategory(Category category)
		{
			db.Entry(category).State = EntityState.Modified;
			db.SaveChanges();
			return category;
		}

		public Category DeletecaCategory(short id)
		{
			Category category = db.Categories.Find(id);
			db.Categories.Remove(category);
			db.SaveChanges();
			return category;
		}
	}
}