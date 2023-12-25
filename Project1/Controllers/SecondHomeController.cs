using Microsoft.AspNetCore.Mvc;

namespace Project1.Controllers
{
	public class SecondHomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
