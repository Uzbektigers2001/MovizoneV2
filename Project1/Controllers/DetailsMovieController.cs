using Microsoft.AspNetCore.Mvc;
using Project1.ViewModels;

namespace Project1.Controllers
{
	public class DetailsMovieController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Index(MediaViewModel viewModel)
		{
			return View(viewModel);
		}
	}
}
