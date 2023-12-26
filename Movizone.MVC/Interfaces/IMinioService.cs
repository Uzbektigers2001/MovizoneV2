using Microsoft.AspNetCore.Mvc;
using Minio.DataModel;

namespace Movizone.MVC.Interfaces
{
	public interface IMinioService
	{
		Task<string> Upload(string bucketName, string folderName, string fileName, Stream stream);
		Task UploadWithoutGuid(string bucketName, string fileAndFolderPath, Stream stream);
		Task<string> Upload(string bucketName, string folderName, IFormFile file);
		Task<string> ReplaceUpload(string fileNameGuid, string bucketName, string folderName, Stream stream);
		Task<Stream> Download(string bucketName, string folderName, string fileNameGuid);
		Task<FileStreamResult> VideoStreaming(string bucketName, string folderName, string fileNameGuid);
		Task<ObjectStat> GetStats(string bucketName, string folderName, string fileNameGuid);
		Task Delete(string bucketName, string folderName, string fileNameGuid);
		Task Delete(string bucketName, IEnumerable<string> fileNames);
	}
}
