namespace Nexer.Weather.Application.Models
{
    public class DeviceData
    {
        public string DeviceId { get; init; }
        public IDictionary<string, IDictionary<DateTime, float>> SensorData { get; init; }

        public DeviceData(string deviceId, IEnumerable<SensorData> sensorData)
        {
            DeviceId = deviceId;
            SensorData = sensorData.ToDictionary(sd => sd.SensorType, sd => sd.SensorValues);
        }
    }
}
