using Microsoft.AspNetCore.Mvc;

namespace Movizone.MVC.Controllers
{
	public class ErrorController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
