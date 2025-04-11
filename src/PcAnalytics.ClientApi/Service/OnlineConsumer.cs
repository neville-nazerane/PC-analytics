using PcAnalytics.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace PcAnalytics.ClientApi.Service
{
    public class OnlineConsumer(HttpClient client)
    {
        private readonly HttpClient _client = client;

        public void SetComputerSerial(string serial)
        {
            _client.DefaultRequestHeaders.Add("computerSerial", serial);
        }

        public async Task UploadAsync(IEnumerable<SensorInput> input,
                                              CancellationToken cancellationToken = default)
        {
            using var response = await _client.PostAsJsonAsync("sensorInputs", input, cancellationToken);
            response.EnsureSuccessStatusCode();
        }

    }
}
