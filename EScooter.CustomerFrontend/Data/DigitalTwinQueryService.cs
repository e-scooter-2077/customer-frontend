using Azure;
using Azure.DigitalTwins.Core;
using EasyDesk.Tools.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using static EasyDesk.Tools.Options.OptionImports;

namespace EScooter.CustomerFrontend.Data
{
    public class DigitalTwinQueryService : IQueryService
    {

        DigitalTwinsClient _client;

        public DigitalTwinQueryService(DigitalTwinsClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<CustomerViewModel>> GetCustomers()
        {
            var customers = new List<CustomerViewModel>();

            string query = "SELECT * FROM DIGITALTWINS DT WHERE IS_OF_MODEL(DT, 'dtmi:com:escooter:Customer;1')";
            AsyncPageable<BasicDigitalTwin> result = _client.QueryAsync<BasicDigitalTwin>(query);
            try
            {
                await foreach (BasicDigitalTwin twin in result)
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
            string id = customerId.ToString();
            AsyncPageable<BasicRelationship> result = _client.GetRelationshipsAsync<BasicRelationship>(id, "is_riding");
            
            await foreach (BasicRelationship rentRelationship in result)
            {
                var rentId = new Guid(rentRelationship.Id);
                var startTime = ((JsonElement)rentRelationship.Properties["start"]).GetDateTime();
                var scooterId = new Guid(rentRelationship.TargetId);
                rent = new RentViewModel(rentId, startTime, customerId, scooterId);
            }

            if(rent == null)
            {
                return None;
            }
            return Some(rent);
        }

        public async Task<IEnumerable<ScooterViewModel>> GetScooters()
        {
            var scooters = new List<ScooterViewModel>();

            string query = "SELECT * FROM DIGITALTWINS DT WHERE IS_OF_MODEL(DT, 'dtmi:com:escooter:EScooter;1')";
            AsyncPageable<BasicDigitalTwin> result = _client.QueryAsync<BasicDigitalTwin>(query);
            try
            {
                await foreach (BasicDigitalTwin twin in result)
                {
                    Guid id = new Guid(twin.Id);

                    var latitude = ((JsonElement)twin.Contents["Latitude"]).GetDouble();
                    var longitude = ((JsonElement)twin.Contents["Longitude"]).GetDouble();
                    var battery = ((JsonElement)twin.Contents["BatteryLevel"]).GetDouble();

                    scooters.Add(new ScooterViewModel(id, latitude, longitude, battery));
                }
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine($"Error {ex.Status}, {ex.ErrorCode}, {ex.Message}");
                throw;
            }
            return scooters;
        }
    }
}
