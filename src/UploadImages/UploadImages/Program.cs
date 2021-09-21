using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace UploadImages
{
	class Program
	{
		static async Task Main()
		{
			String connectionString = "DefaultEndpointsProtocol=https;AccountName=mytnc;AccountKey=HK2cdJ68a+hM6qmstOtEsMD6t3SRwqOLFMY8FdJoeborqIaVFtOC1sHv4SfhVZRENEGO/yRntPzl9ik4IsoVog==;EndpointSuffix=core.windows.net";

			// Create a BlobServiceClient object which will be used to create a container client
			BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

			//Create a unique name for the container
			string containerName = "images";

			// Create the container and return a container client object
			//BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);
			BlobContainerClient containerClient = new BlobContainerClient(connectionString, containerName);

			Console.WriteLine(containerClient.Name);

			// Create a local file in the ./data/ directory for uploading and downloading
			string localPath = "C:/Users/dongl/Desktop/Pictures/";
			//string fileName = "house.jpg";
			//string localFilePath = Path.Combine(localPath, fileName);
			
			string[] localFilePaths = Directory.GetFiles(localPath);
			foreach (var filePath in localFilePaths) {
				// Get a reference to a blob
				var fileName = Path.GetFileName(filePath);
				BlobClient blobClient = containerClient.GetBlobClient(fileName);

				Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);

				// Upload data from the local file
				await blobClient.UploadAsync(filePath, true);
			}

			//list blob items
			//download the blob to local path
			await foreach (BlobItem blobItem in containerClient.GetBlobsAsync()) {

				var downloadPath = "C:/Users/dongl/Desktop/DownloadFromBlob/" + blobItem.Name;
				 BlobClient blobClient = containerClient.GetBlobClient(blobItem.Name);
				Console.WriteLine(blobItem.Properties.ContentLength);
				await blobClient.DownloadToAsync(downloadPath);
			}
		}
	}
}
