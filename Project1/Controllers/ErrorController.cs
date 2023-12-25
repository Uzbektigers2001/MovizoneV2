using Microsoft.AspNetCore.Mvc;

namespace Project1.Controllers
{
	public class ErrorController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
