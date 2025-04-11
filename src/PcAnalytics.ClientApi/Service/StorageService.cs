using PcAnalytics.Models;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;

namespace PcAnalytics.ClientApi.Service
{
    public class StorageService(string path) : IAsyncDisposable, IDisposable
    {
        private readonly SemaphoreSlim _locker = new(1);
        private readonly string _sensorsFileLocation = Path.Combine(path, "sensors.json");

        private Stream? sensorReadStream;
        private Stream? sensorWriteStream;
        private bool _disposed;

        public async Task StoreAsync(IEnumerable<IncomingSensorInput> sensorInputs,
                                     CancellationToken cancellationToken = default)
        {
            for (int i = 0; i < 3; i++) // retry
            {

                await _locker.WaitAsync(cancellationToken);

                try
                {
                    sensorWriteStream ??= File.OpenWrite(_sensorsFileLocation);

                    await JsonSerializer.SerializeAsync(sensorWriteStream, sensorInputs, cancellationToken: cancellationToken);
                    await sensorWriteStream.FlushAsync(cancellationToken);
                    return;
                }
                catch when (!cancellationToken.IsCancellationRequested)
                {
                    if (sensorWriteStream is not null)
                    {
                        await sensorWriteStream.DisposeAsync();
                        sensorWriteStream = null;
                    }
                    await Task.Delay(200, cancellationToken);
                }
                finally
                {
                    _locker.Release();
                }
            }
        }

        public async IAsyncEnumerable<IncomingSensorInput> ReadSensorInputs([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await _locker.WaitAsync(cancellationToken);

            try
            {
                sensorReadStream ??= File.OpenRead(_sensorsFileLocation);

                var res = JsonSerializer.DeserializeAsyncEnumerable<IncomingSensorInput>(sensorReadStream, cancellationToken: cancellationToken);

                await foreach (var input in res)
                    if (input is not null)
                        yield return input;
            }
            finally
            {
                _locker.Release();
            }

        }


        public async ValueTask DisposeAsync()
        {
            if (_disposed) return;

            if (sensorReadStream is not null)
                await sensorReadStream.DisposeAsync();
            if (sensorWriteStream is not null)
                await sensorWriteStream.DisposeAsync();

            _locker.Dispose();
            _disposed = true;

            GC.SuppressFinalize(this);
        }

        public void Dispose()
        {
            if (_disposed) return;

            sensorReadStream?.Dispose();
            sensorWriteStream?.Dispose();

            _locker.Dispose();
            _disposed = true;

            GC.SuppressFinalize(this);
        }

        ~StorageService()
        {
            Dispose();
        }
    }

}
