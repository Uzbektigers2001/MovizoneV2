using Microsoft.AspNetCore.Mvc;

namespace Project1.Controllers
{
	public class CatalogListController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
