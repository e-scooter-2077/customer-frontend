using System;

namespace EScooter.CustomerFrontend.Data
{
    public record RentViewModel(Guid Id, DateTime StartTime, Guid CustomerId, Guid ScooterId);
}
