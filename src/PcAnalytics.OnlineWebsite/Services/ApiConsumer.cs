using PcAnalytics.Models.Entities;
using System.Net.Http.Json;

namespace PcAnalytics.OnlineWebsite.Services
{
    public class ApiConsumer(HttpClient client)
    {
        private readonly HttpClient _client = client;

        public async Task<IEnumerable<Computer>> GetComputerAsync(CancellationToken cancellationToken = default)
            => (await _client.GetFromJsonAsync<IEnumerable<Computer>>("computers", cancellationToken: cancellationToken)) ?? [];

        public async Task<IEnumerable<Hardware>> GetHardwareAsync(int computerId, CancellationToken cancellationToken = default)
            => (await _client.GetFromJsonAsync<IEnumerable<Hardware>>("computer/{computerId}/hardware", cancellationToken: cancellationToken)) ?? [];



    }
}
