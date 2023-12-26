using Microsoft.AspNetCore.Mvc;

namespace Movizone.MVC.Controllers
{
	public class FirstHomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
