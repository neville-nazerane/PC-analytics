using PcAnalytics.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace PcAnalytics.ClientBackground.Services
{
    public class ApiConsumer(HttpClient client)
    {
        private readonly HttpClient _client = client;


        public async Task UploadIncomingAsync(IEnumerable<IncomingSensorInput> input,
                                              CancellationToken cancellationToken = default)
        {
            using var response = await _client.PostAsJsonAsync("incoming", input, cancellationToken);
            response.EnsureSuccessStatusCode();
        }

    }
}
