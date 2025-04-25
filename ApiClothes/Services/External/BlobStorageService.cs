using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;

public class BlobStorageService
{
    private readonly BlobServiceClient _client;
    private readonly string _containerName = "images"; // przykładowy kontener

    public BlobStorageService(IConfiguration config)
    {
        var conn = config["AzureStorage:ConnectionString"];
        _client = new BlobServiceClient(conn);
        // upewnij się, że kontener istnieje:
        var container = _client.GetBlobContainerClient(_containerName);
        container.CreateIfNotExists(PublicAccessType.Blob);
    }

    public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType)
    {
        var container = _client.GetBlobContainerClient(_containerName);
        var blob = container.GetBlobClient(fileName);
        await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = contentType });
        // URL do pliku:
        return blob.Uri.ToString();
    }

    public async Task<Stream> DownloadAsync(string fileName)
    {
        var container = _client.GetBlobContainerClient(_containerName);
        var blob = container.GetBlobClient(fileName);
        var resp = await blob.DownloadAsync();
        return resp.Value.Content;
    }
}
