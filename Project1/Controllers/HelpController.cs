using Microsoft.AspNetCore.Mvc;

namespace Project1.Controllers
{
	public class HelpController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
