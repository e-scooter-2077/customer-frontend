using Azure;
using Azure.DigitalTwins.Core;
using EasyDesk.Tools.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using static EasyDesk.Tools.Options.OptionImports;

namespace EScooter.CustomerFrontend.Data
{
    public record ScooterDto(Guid Id, double Latitude, double Longitude, double BatteryLevel, bool Enabled, bool Rented, bool Locked, bool Standby, bool Connected);

    public class DigitalTwinQueryService : IQueryService
    {
        private readonly DigitalTwinsClient _dtClient;
        private readonly HttpClient _httpClient;
        private readonly string _getScooterUrl = "https://admin-api-gateway.azurewebsites.net/api/scooters";

        public DigitalTwinQueryService(DigitalTwinsClient dtClient, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _dtClient = dtClient;
        }

        public async Task<IEnumerable<CustomerViewModel>> GetCustomers()
        {
            var customers = new List<CustomerViewModel>();

            var query = "SELECT * FROM DIGITALTWINS DT WHERE IS_OF_MODEL(DT, 'dtmi:com:escooter:Customer;1')";
            var result = _dtClient.QueryAsync<BasicDigitalTwin>(query);
            try
            {
                await foreach (var twin in result)
                {
                    customers.Add(new CustomerViewModel(new Guid(twin.Id)));
                }
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine($"Error {ex.Status}, {ex.ErrorCode}, {ex.Message}");
                throw;
            }
            return customers;
        }

        public async Task<Option<RentViewModel>> GetRent(Guid customerId)
        {
            RentViewModel rent = null;
            var id = customerId.ToString();
            var result = _dtClient.GetRelationshipsAsync<BasicRelationship>(id, "is_riding");

            await foreach (var rentRelationship in result)
            {
                var rentId = new Guid(rentRelationship.Id);
                var startTime = ((JsonElement)rentRelationship.Properties["start"]).GetDateTime();
                var scooterId = new Guid(rentRelationship.TargetId);
                rent = new RentViewModel(rentId, startTime, customerId, scooterId);
            }

            if (rent == null)
            {
                return None;
            }
            return Some(rent);
        }

        public async Task<IEnumerable<ScooterViewModel>> GetScooters()
        {
            var scooters = new List<ScooterViewModel>();

            var result = await _httpClient.GetAsync(_getScooterUrl);
            if (!result.IsSuccessStatusCode)
            {
                return scooters;
            }

            var scootersJson = await result.Content.ReadAsStringAsync();
            var scootersDto = JsonSerializer.Deserialize<List<ScooterDto>>(scootersJson);
            scooters = scootersDto
                .Where(x => x.Connected && x.Enabled && !x.Rented && !x.Standby)
                .Select(x => ScooterDtoToViewModel(x)).ToList();

            return scooters;
        }

        private ScooterViewModel ScooterDtoToViewModel(ScooterDto x)
        {
            return new ScooterViewModel(x.Id, x.Latitude, x.Longitude, x.BatteryLevel);
        }
    }
}
