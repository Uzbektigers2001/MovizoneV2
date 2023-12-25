using Microsoft.AspNetCore.Mvc;

namespace Project1.Controllers
{
	public class AboutController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
