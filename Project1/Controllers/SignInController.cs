using Microsoft.AspNetCore.Mvc;

namespace Project1.Controllers
{
	public class SignInController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
