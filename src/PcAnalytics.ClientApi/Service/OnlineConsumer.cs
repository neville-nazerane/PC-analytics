using PcAnalytics.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace PcAnalytics.ClientApi.Service
{
    public class OnlineConsumer
    {
        private readonly HttpClient _client;

        public OnlineConsumer(HttpClient client, InfoService service)
        {
            _client = client;
            _client.DefaultRequestHeaders.Add("computerSerial", service.Serial);

        }

        //public void SetComputerSerial()
        //{
        //}

        public async Task UploadAsync(IEnumerable<SensorInput> input,
                                      CancellationToken cancellationToken = default)
        {
            using var response = await _client.PostAsJsonAsync("sensorInputs", input, cancellationToken);
            response.EnsureSuccessStatusCode();
        }

    }
}
