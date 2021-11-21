using EasyDesk.Tools.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EScooter.CustomerFrontend.Data
{
    public interface IQueryService
    {
        Task<IEnumerable<CustomerViewModel>> GetCustomers();

        Task<IEnumerable<ScooterViewModel>> GetScooters();

        Task<Option<RentViewModel>> GetRent(Guid customerId);
    }
}
