using Microsoft.AspNetCore.Mvc;
using Nexer.Weather.Application.Models;
using Nexer.Weather.Application.Services;
using System.Net;

namespace Nexer.Weather.API
{
    internal static class Router
    {
        public static WebApplication RegisterRoutes(this WebApplication app)
        {
            app.MapGet("api/v1/getdata", async ([FromQuery] string deviceId, [FromQuery] DateTime date, [FromQuery] string sensorType, [FromServices] IDataService storage, CancellationToken cancellationToken) =>
            {
                var result = await storage.GetDataForSensorAsync(deviceId, date, sensorType, cancellationToken);

                if (!result.IsSuccess)
                    return Results.NotFound(result.ErrorMessage);

                return Results.Ok(result.Content);
            })
            .Produces<SensorData>((int)HttpStatusCode.OK, "application/json")
            .Produces((int)HttpStatusCode.NotFound);

            app.MapGet("api/v1/getdatafordevice", async ([FromQuery] string deviceId, [FromQuery] DateTime date, [FromServices] IDataService storage, CancellationToken cancellationToken) =>
            {
                var result = await storage.GetDataForDeviceAsync(deviceId, date, cancellationToken);

                if (!result.IsSuccess)
                    return Results.NotFound(result.ErrorMessage);

                return Results.Ok(result.Content);
            })
            .Produces<DeviceData>((int)HttpStatusCode.OK, "application/json")
            .Produces((int)HttpStatusCode.NotFound);

            return app;
        }
    }
}
