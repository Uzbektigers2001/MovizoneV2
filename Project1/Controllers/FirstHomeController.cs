using Microsoft.AspNetCore.Mvc;

namespace Project1.Controllers
{
	public class FirstHomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
