using System;
using System.Threading.Tasks;

namespace EScooter.CustomerFrontend.Data
{
    public interface IRentService
    {
        Task<RentViewModel> StartRent(Guid customerId, Guid scooterId);

        Task StopRent(Guid rentId);
    }
}
