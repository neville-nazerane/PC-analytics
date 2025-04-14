using PcAnalytics.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace PcAnalytics.ClientBackground.Services
{
    public class LocalApiConsumer(HttpClient client)
    {
        
        private readonly HttpClient _client = client;

        public async Task AddAsync(IEnumerable<SensorInput> inputs, CancellationToken cancellationToken = default)
        {
            using var res = await _client.PostAsJsonAsync("sensorInputs", inputs, cancellationToken);
            res.EnsureSuccessStatusCode();
        }
        
        public async Task UploadAsync(CancellationToken cancellationToken = default)
        {
            using var res = await _client.PostAsync("sensorInputs/uploadStored", null, cancellationToken);
            res.EnsureSuccessStatusCode();
        }

    }
}
