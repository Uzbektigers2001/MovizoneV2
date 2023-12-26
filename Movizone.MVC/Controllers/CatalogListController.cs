using Microsoft.AspNetCore.Mvc;

namespace Movizone.MVC.Controllers
{
	public class CatalogListController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
