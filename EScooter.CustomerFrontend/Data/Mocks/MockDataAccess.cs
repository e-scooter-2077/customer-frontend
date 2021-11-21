using EasyDesk.Tools.Collections;
using EasyDesk.Tools.Options;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using static EasyDesk.Tools.Collections.ImmutableCollections;

namespace EScooter.CustomerFrontend.Data.Mocks
{
    public class MockDataAccess : IQueryService, IRentService
    {
        private readonly IImmutableList<CustomerViewModel> _customers;
        private readonly IImmutableList<ScooterViewModel> _scooters;

        private IImmutableDictionary<Guid, RentViewModel> _rents;

        public MockDataAccess()
        {
            _customers = List(
                new CustomerViewModel(Guid.NewGuid()),
                new CustomerViewModel(Guid.NewGuid()),
                new CustomerViewModel(Guid.NewGuid()));

            _scooters = List(
                new ScooterViewModel(Guid.NewGuid(), 34.4129481, 84.10239, 30.0),
                new ScooterViewModel(Guid.NewGuid(), 34.4890483, 84.03129, 80.0));

            _rents = Map<Guid, RentViewModel>();
        }

        public Task<IEnumerable<CustomerViewModel>> GetCustomers() =>
            Task.FromResult<IEnumerable<CustomerViewModel>>(_customers);

        public Task<IEnumerable<ScooterViewModel>> GetScooters() =>
            Task.FromResult<IEnumerable<ScooterViewModel>>(_scooters);

        public Task<Option<RentViewModel>> GetRent(Guid customerId) =>
            Task.FromResult(_rents.GetOption(customerId));

        public Task<RentViewModel> StartRent(Guid customerId, Guid scooterId)
        {
            var rent = new RentViewModel(Guid.NewGuid(), DateTime.UtcNow, customerId, scooterId);
            _rents = _rents.Add(customerId, rent);
            return Task.FromResult(rent);
        }

        public Task StopRent(Guid rentId)
        {
            var customerId = _rents
                .Where(r => r.Value.RentId == rentId)
                .Select(r => r.Key)
                .FirstOrDefault();

            _rents = _rents.Remove(customerId);

            return Task.CompletedTask;
        }
    }
}
