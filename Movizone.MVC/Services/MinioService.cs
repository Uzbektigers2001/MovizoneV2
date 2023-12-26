using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.DataModel;
using Minio.DataModel.Args;
using Movizone.MVC.Interfaces;
using System.IO;
using System.Net.Sockets;

namespace Movizone.MVC.Services
{
	public class MinioService : IMinioService
	{
		private static readonly string[] GrantedFileTypes =
		{
			".pdf", ".docx", ".ftlh", ".html", ".jpg", ".xlsx", ".xls",
			".jfif", ".jpe", ".jpeg", ".png"
		};
		private static readonly string[] videoExtensions =
		{
			".mp4", ".avi", ".mov", ".wmv", ".mkv", ".flv", ".webm",
			".mpeg", ".mpg", ".3gp", ".ogv", ".vob", ".ts", ".m4v"
			// Qo'shimcha formatlarni qo'shing
		};

		private static readonly string[] audioExtensions =
		{
			".wav", ".mp3", ".aac", ".flac", ".m4a", ".m4p", ".oga",
			".ogg", ".ogv", ".ogx", ".spx"
			// Qo'shimcha formatlarni qo'shing
		};

		private static readonly string[] photoExtensions =
		{
			".bmp", ".ico", ".gif", ".jpeg", ".jpg", ".png", ".webp",
			".tiff", ".tif", ".jif", ".jfif", ".jp2", ".jpx", ".j2k",
			".j2c", ".fpx", ".pcd", ".svg", ".svgz", ".eps", ".ai",
			".dxf", ".pdf", ".ps", ".psd", ".rel", ".tab", ".epr", ".cbr",
			".cbz", ".cb7", ".cbt", ".cba", ".djvu", ".djv", ".fb2",
			// Qo'shimcha formatlarni qo'shing
		};

		private readonly IMinioClient _minio;

		public MinioService(IMinioClient minio)
		{
			_minio = minio;
		}

		/// <summary>
		/// Method uploads data to MinIO server.
		/// </summary>
		/// <param name="bucketName">Name of the directory in MinIO, in what to save file.</param>
		/// <param name="folderName">Folder name in what this file will be saved.</param>
		/// <param name="fileName">Original file name with extension.</param>
		/// <param name="stream">Just a stream.</param>
		/// <returns>Returns new file name that is GUID.</returns>
		/// <exception cref="Exception">Not supported file type. Supported: .pdf, .html, .docx, .ftlh</exception>
		public async Task<string> Upload(string bucketName, string folderName, string fileName, Stream stream)
		{
			string fileExtension = fileName.Substring(fileName.LastIndexOf('.')).ToLower();
			if (!videoExtensions.Contains(fileExtension))
			{
				throw new Exception("Not supported file type.");
			}

			string fileNameGuid = Guid.NewGuid().ToString();
			string newFileName = folderName + '/' + fileNameGuid + "--" + fileName;

			PutObjectArgs putObjectArgsStream = new PutObjectArgs()
				.WithBucket(bucketName)
				.WithObject(newFileName.Trim())
				.WithStreamData(stream)
				.WithObjectSize(stream.Length)
				.WithContentType("application/octet-stream");

			await _minio.PutObjectAsync(putObjectArgsStream);

			return fileNameGuid + "--" + fileName;
		}

		/// <summary>
		/// Method uploads data to MinIO server.
		/// </summary>
		/// <param name="bucketName">Name of the directory in MinIO, in what to save file.</param>
		/// <param name="name">Object name.</param>
		/// <param name="fileAndFolderPath">Folder and file name path which have to save.</param>
		/// <param name="stream">Just a stream.</param>
		/// <returns>Returns new file name that is GUID.</returns>
		/// <exception cref="Exception">Not supported file type. Supported: .pdf, .html, .docx, .ftlh</exception>
		public async Task UploadWithoutGuid(string bucketName, string fileAndFolderPath, Stream stream)
		{
			try
			{
				var objectArgs = new PutObjectArgs()
					.WithBucket(bucketName)
					.WithStreamData(stream)
					.WithObject(fileAndFolderPath)
					.WithObjectSize(stream.Length);
				await _minio.PutObjectAsync(objectArgs);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}

		}

		/// <summary>
		/// Method uploads data to MinIO server.
		/// </summary>
		/// <param name="bucketName">Name of the directory in MinIO, in what to save file.</param>
		/// <param name="folderName">Folder name in what this file will be saved.</param>
		/// <param name="file">IFormFile to save it in MinIO and database.</param>
		/// <returns>Returns new file name that is GUID.</returns>
		/// <exception cref="Exception">Not supported file type. Supported: .pdf, .doc, .docx, .ftlh</exception>
		public async Task<string> Upload(string bucketName, string folderName, IFormFile file)
		{
			string fileName = file.FileName;

			string fileExtension = fileName.Substring(fileName.LastIndexOf('.'));
			if (!videoExtensions.Contains(fileExtension))
			{
				throw new Exception("Not supported file type.");
			}

			string fileNameGuid = Guid.NewGuid().ToString();
			string newFileName = folderName + '/' + fileNameGuid + ':' + fileName;

			Stream fileStream = file.OpenReadStream();

			PutObjectArgs putObjectArgsStream = new PutObjectArgs()
				.WithBucket(bucketName)
				.WithObject(newFileName)
				.WithStreamData(fileStream)
				.WithObjectSize(fileStream.Length)
				.WithContentType("application/octet-stream");

			await _minio.PutObjectAsync(putObjectArgsStream);

			return fileNameGuid + ':' + fileName;
		}


		/// <summary>
		/// Method replace with old object data to MinIO server.
		/// </summary>
		/// <param name="fileNameGuid">Name of the directory in MinIO, in what to save file.</param>
		/// <param name="bucketName">Name of the directory in MinIO, in what to save file.</param>
		/// <param name="folderName">Folder name in what this file will be saved.</param>
		/// <param name="stream">Just a stream.</param>
		/// <returns>Returns new file name that is GUID.</returns>
		/// <exception cref="Exception">Not supported file type. Supported: .pdf, .html, .docx, .ftlh</exception>
		public async Task<string> ReplaceUpload(string fileNameGuid, string bucketName, string folderName, Stream stream)
		{
			var objectArgs = new RemoveObjectArgs()
				.WithBucket(bucketName)
				.WithObject(folderName + '/' + fileNameGuid);

			await _minio.RemoveObjectAsync(objectArgs);

			await this.UploadWithoutGuid(bucketName, folderName + '/' + fileNameGuid, stream);

			return fileNameGuid;
		}

		/// <summary>
		/// Method downloads file stream from MinIO server.
		/// </summary>
		/// <param name="bucketName">Name of the directory in MinIO, in what to save file.</param>
		/// <param name="folderName">Folder name in what this file will be saved.</param>
		/// <param name="fileNameGuid">File name at MinIO server, that is GUID.</param>
		/// <returns>File stream.</returns>
		/// <exception cref="Exception">If there is no file with given name.</exception>
		public async Task<Stream> Download(string bucketName, string folderName, string fileNameGuid)
		{
			Stream fileData = new MemoryStream();
			//fileData.Position = 0;

			var builderParams = new GetObjectArgs()
				.WithBucket(bucketName)
				.WithObject(folderName + '/' + fileNameGuid)
				.WithCallbackStream(stream => stream.CopyTo(fileData));

			await _minio.GetObjectAsync(builderParams);

			if (fileData == null)
			{
				throw new Exception("Incorrect file name.");
			}
			fileData.Position = 0;
			return fileData;
		}

		/// <summary>
		/// Method returns meta of the file in MinIO server.
		/// </summary>
		/// <param name="bucketName">Name of the directory in MinIO, in what to save file.</param>
		/// <param name="folderName">Folder name in what this file will be saved.</param>
		/// <param name="fileNameGuid">File name at MinIO server, that is GUID.</param>
		/// <returns>ObjectStat object with file meta.</returns>
		public async Task<ObjectStat> GetStats(string bucketName, string folderName, string fileNameGuid)
		{
			StatObjectArgs statObjectArgs = new StatObjectArgs()
				.WithBucket(bucketName)
				.WithObject(folderName + '/' + fileNameGuid);

			return await _minio.StatObjectAsync(statObjectArgs);
		}

		/// <summary>
		/// Method deletes file from MinIO server.
		/// </summary>
		/// <param name="bucketName">Name of the directory in MinIO, in what to save file.</param>
		/// <param name="folderName">Folder name in what this file will be saved.</param>
		/// <param name="fileNameGuid">File name at MinIO server, that is GUID.</param>
		public async Task Delete(string bucketName, string folderName, string fileNameGuid)
		{
			var objectArgs = new RemoveObjectArgs()
				.WithBucket(bucketName)
				.WithObject(folderName + '/' + fileNameGuid);

			await _minio.RemoveObjectAsync(objectArgs);
		}

		/// <summary>
		/// Method deletes group of files from MinIO server.
		/// </summary>
		/// <param name="bucketName">Name of the directory in MinIO, in what to save file.</param>
		/// <param name="fileNames">List of file names represent by GUID path. Example: "documents/79e05c3d-3b11-44d4-88a0-9f4fbd67bf4c.pdf"</param>
		public async Task Delete(string bucketName, IEnumerable<string> fileNames)
		{
			foreach (var fileName in fileNames)
			{
				var objectArgs = new RemoveObjectArgs()
					.WithBucket(bucketName)
					.WithObject(fileName);

				await _minio.RemoveObjectAsync(objectArgs);
			}
		}

		public async Task<string?> PresignedUrl(string bucketName, string folderName, string fileName)
		{
			if (_minio is null) throw new ArgumentNullException(nameof(_minio));

			var reqParams = new Dictionary<string, string>(StringComparer.Ordinal)
			{
				{ "response-content-type", "application/pdf" }
			};

			var args = new PresignedGetObjectArgs()
				.WithBucket(bucketName)
				.WithObject(folderName + '/' + fileName)
				.WithExpiry(60000);
				//.WithHeaders(reqParams);
			var presignedUrl = await _minio.PresignedGetObjectAsync(args).ConfigureAwait(false);

			return presignedUrl;
		}

		public async Task<FileStreamResult> VideoStreaming(string bucketName, string folderName, string fileNameGuid)
		{
			Stream fileData = new MemoryStream();
			//fileData.Position = 0;

			var builderParams = new GetObjectArgs()
				.WithBucket(bucketName)
				.WithObject(folderName + '/' + fileNameGuid)
				.WithCallbackStream(stream => stream.CopyTo(fileData));

			await _minio.GetObjectAsync(builderParams);

			if (fileData == null)
			{
				throw new Exception("Incorrect file name.");
			}

			var mimeType = $"video/{fileNameGuid.Split('.').Last().ToLower().Replace(".","")}";

			var result = new FileStreamResult(fileData, mimeType)
			{
				FileDownloadName = fileNameGuid.Split(":").Last(),
				EnableRangeProcessing = true
			};

			return result;
		}
	}
}
