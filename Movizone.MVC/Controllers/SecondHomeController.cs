using Microsoft.AspNetCore.Mvc;

namespace Movizone.MVC.Controllers
{
	public class SecondHomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
