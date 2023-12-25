using Microsoft.AspNetCore.Mvc;

namespace Project1.Controllers
{
	public class CatalogGridController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
