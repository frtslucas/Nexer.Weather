namespace Nexer.Weather.Application.Models
{
    public class SensorData
    {
        public string DeviceId { get; init; }
        public string SensorType { get; init; }
        public IDictionary<DateTime, float> SensorValues { get; init; }

        public SensorData(string deviceId, string sensorType, string csv)
        {
            DeviceId = deviceId;
            SensorType = sensorType;

            SensorValues = csv.ReadLines().Select(line =>
            {
                var split = line.Split(';');
                return new KeyValuePair<DateTime, float>(DateTime.Parse(split[0]), float.Parse(split[1]));
            })
            .ToDictionary(sv => sv.Key, sv => sv.Value);
        }
    }
}
