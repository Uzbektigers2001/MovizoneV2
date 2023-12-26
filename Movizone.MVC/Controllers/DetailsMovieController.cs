using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Minio;
using Minio.DataModel.Args;
using Movizone.MVC.Interfaces;
using Movizone.MVC.Services;
using Movizone.MVC.ViewModels;
using System.Net.Mime;

namespace Movizone.MVC.Controllers
{
	public class DetailsMovieController : Controller
	{
		private readonly IMinioService _minio;
		private readonly IWebHostEnvironment _appEnvironment;
		public DetailsMovieController(IMinioService minio, IWebHostEnvironment appEnvironment)
		{
			_minio = minio;
			_appEnvironment = appEnvironment;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Index(MediaViewModel viewModel)
		{
			return View(viewModel);
		}

		[HttpGet]
		public async Task<IActionResult> Upload()
		{
			var mimeType = "video/mp4"; // Fayl turi (misol: video/mp4)

			var bucket = "demobucket";

			var videoPath = Path.Combine(_appEnvironment.WebRootPath, "LinkinPark.mp4");

			var fileProvider = new PhysicalFileProvider(Path.GetDirectoryName(videoPath));

			var fileInfo = fileProvider.GetFileInfo(Path.GetFileName(videoPath));

			var fileStream = fileInfo.CreateReadStream();

			await _minio.UploadWithoutGuid(bucket, "LinkinPark.mp4", fileStream);

			return View();
		}

		public async Task<IActionResult> StreamVideo()
		{
			try
			{
				var mimeType = "video/mp4"; // Fayl turi (misol: video/mp4)

				var bucket = "demobucket";

				//var filenameGuid = "65cc8ce0-73d8-4ae2-9da2-e6d4042d9839--LinkinPark-Copy.mp4";

				//var mimeType = $"video/{nameGuid.Split('.').Last().ToLower().Replace(".", "")}";

				return await _minio.VideoStreaming(bucket, "", "LinkinPark.mp4");

				//var result = new FileStreamResult(fileStream, mimeType)
				//{
				//	FileDownloadName = Path.GetFileName(videoPath),
				//	EnableRangeProcessing = true
				//};
				//return result;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message, ex);
			}
		}
	}
}
