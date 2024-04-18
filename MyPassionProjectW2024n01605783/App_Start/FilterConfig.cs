using System.Web;
using System.Web.Mvc;

namespace MyPassionProjectW2024n01605783
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
