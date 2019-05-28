using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccessoriesWebShop.Models
{
	public class WholeUser
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public WholeUser()
		{
		}

		public int id { get; set; }
		public string name { get; set; }
		public string phone { get; set; }
		public string email { get; set; }
		public byte roleID { get; set; }
		public string password { get; set; }

		public virtual Role Role { get; set; }
	}
}