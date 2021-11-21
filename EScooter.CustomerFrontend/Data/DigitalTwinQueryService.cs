using EasyDesk.Tools.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EScooter.CustomerFrontend.Data
{
    class DigitalTwinQueryService : IQueryService
    {
        public DigitalTwinQueryService()
        {

        }
        public Task<IEnumerable<CustomerViewModel>> GetCustomers()
        {
            throw new NotImplementedException();
        }

        public Task<Option<RentViewModel>> GetRent(Guid customerId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ScooterViewModel>> GetScooters()
        {
            throw new NotImplementedException();
        }
    }
}
