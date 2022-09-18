using Nexer.Weather.Application.Models;

namespace Nexer.Weather.Application.Services
{
    public interface IDataService
    {
        Task<Result<SensorData>> GetDataForSensorAsync(string deviceId, DateTime date, string sensorType, CancellationToken cancellationToken = default);
        Task<Result<DeviceData>> GetDataForDeviceAsync(string deviceId, DateTime date, CancellationToken cancellationToken = default);
    }
}
