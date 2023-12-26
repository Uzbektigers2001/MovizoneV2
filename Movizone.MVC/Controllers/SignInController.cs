using Microsoft.AspNetCore.Mvc;

namespace Movizone.MVC.Controllers
{
	public class SignInController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
