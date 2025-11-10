using System.Web;
using System.Web.Mvc;

namespace Chapter02_ProjectFiles
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}
	}
}
