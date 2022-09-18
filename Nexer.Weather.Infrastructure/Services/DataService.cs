using Azure.Storage.Blobs;
using Nexer.Weather.Application;
using Nexer.Weather.Application.Models;
using Nexer.Weather.Application.Services;
using System.IO.Compression;
using System.Text;

namespace Nexer.Weather.Infrastructure.Services
{
    internal sealed class DataService : IDataService
    {
        private static readonly string[] SENSORTYPES = new[]
        { 
            "temperature",
            "rainfall",
            "humidity"
        };

        private readonly BlobContainerClient _blobContainerClient;

        public DataService(BlobContainerClient blobContainerClient)
        {
            _blobContainerClient = blobContainerClient;
        }

        public async Task<Result<SensorData>> GetDataForSensorAsync(string deviceId, DateTime date, string sensorType, CancellationToken cancellationToken = default)
        {
            var csvFileName = GetCSVFileName(deviceId, date, sensorType);
            var csvClient = _blobContainerClient.GetBlobClient(csvFileName);

            string content;
            if (await csvClient.ExistsAsync(cancellationToken))
            {
                using var memoryStream = new MemoryStream();
                await csvClient.DownloadToAsync(memoryStream, cancellationToken);
                content = Encoding.UTF8.GetString(memoryStream.ToArray());
            }
            else
            {
                var zipClient = _blobContainerClient.GetBlobClient(GetZipFileName(deviceId, sensorType));

                if (!await zipClient.ExistsAsync(cancellationToken))
                    return Result<SensorData>.Error("Invalid DeviceId and/or SensorType");

                var zipStream = await zipClient.OpenReadAsync(cancellationToken: cancellationToken);
                using var package = new ZipArchive(zipStream, ZipArchiveMode.Read);
                var zipEntry = package.Entries.SingleOrDefault(entry => entry.Name == Path.GetFileName(csvFileName));

                if (zipEntry is null)
                    return Result<SensorData>.Error($"Data not found for device '{deviceId}' and sensorType '{sensorType}' on date '{date}'");

                using var memoryStream = new MemoryStream();
                using var fileStream = zipEntry.Open();
                fileStream.CopyTo(memoryStream);
                content = Encoding.UTF8.GetString(memoryStream.ToArray());
            }

            return Result<SensorData>.Success(new(deviceId, sensorType, content));
        }

        public async Task<Result<DeviceData>> GetDataForDeviceAsync(string deviceId, DateTime date, CancellationToken cancellationToken = default)
        {
            if (!_blobContainerClient.GetBlobs(prefix: $"{deviceId}").Any())
                return Result<DeviceData>.Error("Invalid DeviceId");

            var allSensorData = new List<SensorData>();

            await Parallel.ForEachAsync(SENSORTYPES, async (sensorType, cancellationToken) =>
            {
                var sensorResult = await GetDataForSensorAsync(deviceId, date, sensorType, cancellationToken);

                if (sensorResult.IsSuccess)
                    allSensorData.Add(sensorResult.Content);
            });

            if (!allSensorData.Any())
                return Result<DeviceData>.Error($"Data not found for device '{deviceId}' on date '{date}'");

            return Result<DeviceData>.Success(new(deviceId, allSensorData));
        }

        private static string GetCSVFileName(string deviceId, DateTime date, string sensorType) => $"/{deviceId}/{sensorType}/{date:yyyy-MM-dd}.csv";
        private static string GetZipFileName(string deviceId, string sensorType) => $"/{deviceId}/{sensorType}/historical.zip";
    }
}
