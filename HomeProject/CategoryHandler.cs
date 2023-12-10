using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeProject
{
	public class CategoryHandler
	{
		public enum Category
		{
			Restaurant = 1,
			Retail = 2,
			Hotel = 3,
			Activity = 4
		}

		private static HashSet<int> validCategories = new HashSet<int> { (int)Category.Restaurant, (int)Category.Retail, (int)Category.Activity };

		public static bool IsValidCategory(int category)
		{
			return validCategories.Contains(category);
		}
	}
}
