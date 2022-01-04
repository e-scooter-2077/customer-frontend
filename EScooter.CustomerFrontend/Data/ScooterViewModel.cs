using System;

namespace EScooter.CustomerFrontend.Data
{
    public record ScooterViewModel(Guid Id, double Latitude, double Longitude, double BatteryLevel, bool rentable);
}
