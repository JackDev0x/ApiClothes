using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Options;

public class GoogleCloudStorageConfig
{
    public string BucketName { get; set; } = string.Empty;
    public string CredentialPath { get; set; } = string.Empty;
}

//public class GoogleCloudStorageService
//{
//    private readonly StorageClient _storageClient;
//    private readonly GoogleCloudStorageConfig _config;

//    public GoogleCloudStorageService(
//        StorageClient storageClient,
//        IOptions<GoogleCloudStorageConfig> config)
//    {
//        _storageClient = storageClient;
//        _config = config.Value;
//    }

//    public async Task UploadFileAsync(string filePath, string destinationObjectName)
//    {
//        using var fileStream = File.OpenRead(filePath);
//        await _storageClient.UploadObjectAsync(
//            _config.BucketName,
//            destinationObjectName,
//            null,
//            fileStream);

//        Console.WriteLine($"Uploaded {filePath} to {_config.BucketName}/{destinationObjectName}");
//    }

//    public async Task DownloadFileAsync(string objectName, string destinationFilePath)
//    {
//        using var outputStream = File.OpenWrite(destinationFilePath);
//        await _storageClient.DownloadObjectAsync(
//            _config.BucketName,
//            objectName,
//            outputStream);

//        Console.WriteLine($"Downloaded {_config.BucketName}/{objectName} to {destinationFilePath}");
//    }
//}
