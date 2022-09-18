using Nexer.Weather.Application.Models;

namespace Nexer.Weather.Tests
{
    public class DataTest
    {
        [Fact]
        public void Should_Construct_DeviceData_From_CSV()
        {
            var csv = new[]
            {
                "2019-01-10T00:01:05;-,62",
                "2019-01-10T00:01:10;-,62",
                "2019-01-10T00:01:15;-,62",
                "2019-01-10T00:01:20;-,62",
                "2019-01-10T00:01:25;-,62",
                "2019-01-10T00:01:30;-,62",
                "2019-01-10T00:01:35;-,62",
                "2019-01-10T00:01:40;-,62",
                "2019-01-10T00:01:45;-,62",
                "2019-01-10T00:01:50;-,63"
            };

            var deviceData = GetMockDeviceData("dockan", "temperature", string.Join(Environment.NewLine, csv));

            Assert.Equal("temperature", deviceData.SensorData.First().Key);
            Assert.Equal(csv.Length, deviceData.SensorData.First().Value.Count);
        }

        [Fact]
        public void Should_Construct_SensorData_FromCSV()
        {
            var csv = new[]
{
                "2019-01-10T00:01:05;-,62",
                "2019-01-10T00:01:10;-,62",
                "2019-01-10T00:01:15;-,62",
                "2019-01-10T00:01:20;-,62",
                "2019-01-10T00:01:25;-,62",
                "2019-01-10T00:01:30;-,62",
                "2019-01-10T00:01:35;-,62"
            };

            var sensorData = new SensorData("dockan", "temperature", string.Join(Environment.NewLine, csv));

            Assert.Equal("temperature", sensorData.SensorType);
            Assert.Equal(csv.Length, sensorData.SensorValues.Count);
        }

        private static DeviceData GetMockDeviceData(string deviceId, string sensorType, string csv) => new(deviceId, new[] { new SensorData(deviceId, sensorType, csv) });
    }
}