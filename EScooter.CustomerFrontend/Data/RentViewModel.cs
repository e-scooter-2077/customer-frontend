using System;

namespace EScooter.CustomerFrontend.Data
{
    public record RentViewModel(Guid RentId, DateTime StartTime, Guid CustomerId, Guid ScooterId);
}
