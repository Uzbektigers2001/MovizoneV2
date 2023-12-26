using Microsoft.AspNetCore.Mvc;

namespace Movizone.MVC.Controllers
{
	public class HelpController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
