using EasyDesk.Tools;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace EScooter.CustomerFrontend.Data
{
    public class HttpRentService : IRentService
    {
        private readonly HttpClient _client;

        public HttpRentService(HttpClient client)
        {
            _client = client;
        }

        private record RentDto(Guid Id, ConfirmationDto Confirmation, CancellationDto Cancellation);

        private record ConfirmationDto(DateTime Timestamp);

        private record CancellationDto(string Reason);

        public async Task<RentViewModel> StartRent(Guid customerId, Guid scooterId)
        {
            var rent = await MakeRequestWithResponse<RentDto>(c => c.PostAsJsonAsync("rents", new { customerId, scooterId }));
            var rentId = rent.Id;
            while (rent.Confirmation is null && rent.Cancellation is null)
            {
                rent = await MakeRequestWithResponse<RentDto>(c => c.GetAsync($"rents/{rentId}"));
            }
            if (rent.Cancellation is not null)
            {
                throw new Exception($"Rent was cancelled (Reason: {rent.Cancellation.Reason})");
            }
            return new RentViewModel(rentId, rent.Confirmation.Timestamp, customerId, scooterId);
        }

        public async Task StopRent(Guid rentId)
        {
            await MakeRequest(c => c.PostAsJsonAsync($"rents/{rentId}/stop", new { }));
        }

        private async Task<T> MakeRequestWithResponse<T>(AsyncFunc<HttpClient, HttpResponseMessage> request)
        {
            var response = await MakeRequest(request);
            return await response.Content.ReadFromJsonAsync<T>();
        }

        private async Task<HttpResponseMessage> MakeRequest(AsyncFunc<HttpClient, HttpResponseMessage> request)
        {
            var response = await request(_client);
            response.EnsureSuccessStatusCode();
            return response;
        }
    }
}
