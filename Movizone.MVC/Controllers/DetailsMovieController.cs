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

		public DetailsMovieController(IMinioService minio)
		{
			_minio = minio;
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

		//[HttpPost]
		//public async Task<IActionResult> UploadFile()
		//{
		//	var bucketNomi = "sizning-bucket-nomingiz"; // O'z bucket nomingizni belgilang
		//	var faylNomi = "misol.mp4"; // Yuklanayotgan fayl nomi

		//	using (var stream = new MemoryStream())
		//	{
		//		var videoPath = "D:/Development/Projects/Project1/Project1/wwwroot/LinkinPark.mp4"; // Video fayl manzili

		//		var fileProvider = new PhysicalFileProvider(Path.GetDirectoryName(videoPath));
		//		var fileInfo = fileProvider.GetFileInfo(Path.GetFileName(videoPath));

		//		var fileStream = fileInfo.CreateReadStream();

		//		var mimeType = "video/mp4"; // Fayl turi (misol: video/mp4)

		//		var bucket = "demobucket";

		//		//var bktExsistArgs = new BucketExistsArgs().WithBucket(bucket);
		//		//bool found = await _minio.BucketExistsAsync(bktExsistArgs);
		//		//if (!found)
		//		//{
		//		//	var mkbktArgs = new MakeBucketArgs().WithBucket(bucket);
		//		//	await _minio.MakeBucketAsync(mkbktArgs);
		//		//}

		//		#region file
		//		//PutObjectArgs putObjectArgs = new PutObjectArgs()
		//		//	.WithBucket(bucket)
		//		//	.WithObject(Path.GetFileName(videoPath))
		//		//	.WithFileName(videoPath)
		//		//	.WithContentType(mimeType);
		//		//await _minio.PutObjectAsync(putObjectArgs);
		//		#endregion

		//		#region stream
		//		PutObjectArgs putObjectArgsStream = new PutObjectArgs()
		//			.WithBucket(bucket)
		//			.WithObject(Path.GetFileName(videoPath))
		//			.WithStreamData(fileStream)
		//			.WithObjectSize(fileStream.Length)
		//			.WithContentType("application/octet-stream");

		//		await _minio.PutObjectAsync(putObjectArgsStream);
		//		#endregion
		//		return View();
		//	}
		//}

		public async Task<IActionResult> StreamVideo()
		{
			try
			{
				//var videoPath = "Development/Projects/Project1/Project1/wwwroot/LinkinPark-Copy.mp4"; // Video fayl manzili

				//var fileProvider = new PhysicalFileProvider(Path.GetDirectoryName(videoPath));
				//var fileInfo = fileProvider.GetFileInfo(Path.GetFileName(videoPath));

				//var fileStream = fileInfo.CreateReadStream();

				//var mimeType = "video/mp4"; // Fayl turi (misol: video/mp4)

				var bucket = "demobucket";
				//var filenameGuid = await _minio.Upload(bucket, Path.GetDirectoryName(videoPath).Replace("\\", "/"), Path.GetFileName(videoPath), fileStream);

				var filenameGuid = "65cc8ce0-73d8-4ae2-9da2-e6d4042d9839--LinkinPark-Copy.mp4";

				var mimeType = $"video/{filenameGuid.Split('.').Last().ToLower().Replace(".", "")}";

				return await _minio.VideoStreaming(bucket, "", "LinkinPark.mp4");
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message, ex);
			}
		}
	}
}
