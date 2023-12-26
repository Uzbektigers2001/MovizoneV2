using Microsoft.AspNetCore.Mvc;

namespace Movizone.MVC.Controllers
{
	public class CatalogGridController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
