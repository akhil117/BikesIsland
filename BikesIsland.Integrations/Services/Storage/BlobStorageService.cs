using Azure;
using Azure.Storage.Blobs;
using BikesIsland.Configurations.Models;
using BikesIsland.Integrations.Interfaces.Storage;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace BikesIsland.Integrations.Services.Storage
{
    public class BlobStorageService : IBlobStorageService
    {

        private readonly BlobServiceClient _blobServiceClient;
        private readonly Configure _configure;
        public BlobStorageService(BlobServiceClient blobServiceClient, IOptions<Configure> configure)
        {
            _blobServiceClient = blobServiceClient ?? throw new ArgumentNullException(nameof(blobServiceClient));
            _configure = configure.Value;
        }
        public Task DeleteBlobIfExistsAsync(string blobName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DoesBlobExistAsync(string blobName)
        {
            throw new NotImplementedException();
        }

        public Task DownloadBlobIfExistsAsync(Stream stream, string blobName)
        {
            throw new NotImplementedException();
        }

        public string GenerateSasTokenForContainer()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetBlobUrl(string blobName)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UploadBlobAsync(Stream stream, string blobName)
        {
            try
            {
                Debug.Assert(stream.CanSeek);
                var seekOrigin = SeekOrigin.Begin;
                stream.Seek(0, SeekOrigin.Begin);
                var container = await GetBlobContainer();

                BlobClient blob = container.GetBlobClient(blobName);
                await blob.UploadAsync(stream);
                Log.Information($"Image has been uploaded successfully... Here is the image uri {blob.Uri.AbsoluteUri}");
                return blob.Uri.AbsoluteUri;
            }catch(Exception e)
            {
                Log.Error($"Image has been upload failed... Here is the image uri {e.Message}");

                return null;
            }
        }



        private async Task<BlobContainerClient> GetBlobContainer()
        {
            try
            {
                BlobContainerClient container = _blobServiceClient
                                .GetBlobContainerClient(_configure.BlobStorageSettings.ContainerName);

                await container.CreateIfNotExistsAsync();

                return container;
            }
            catch (RequestFailedException ex)
            {
                Log.Error($"Cannot find blob container: {_configure.BlobStorageSettings.ContainerName} - error details: {ex.Message}");
                throw;
            }
        }
    }
}
