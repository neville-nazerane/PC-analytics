using PcAnalytics.Models;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace PcAnalytics.ClientApi.Service
{
    public class StorageService(string path) : IAsyncDisposable, IDisposable
    {
        private readonly SemaphoreSlim _locker = new(1);
        private readonly string _sensorsFileLocation = Path.Combine(path, "sensors.json");

        private Stream? _sensorReadStream;
        private Stream? _sensorWriteStream;
        private bool _disposed;


        public async Task StoreAsync(IEnumerable<IncomingSensorInput> incomingSensorInputs,
                                     CancellationToken cancellationToken = default)
        {
            await _locker.WaitAsync(cancellationToken);

            try
            {
                _sensorWriteStream ??= File.OpenWrite(_sensorsFileLocation);
                await JsonSerializer.SerializeAsync(_sensorWriteStream, incomingSensorInputs, cancellationToken: cancellationToken);
                await _sensorWriteStream.FlushAsync(cancellationToken);
            }
            finally
            {
                _locker.Release();
            }
        }

        public async IAsyncEnumerable<IncomingSensorInput> ReadSensorInputs([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await _locker.WaitAsync(cancellationToken);

            try
            {
                _sensorReadStream ??= File.OpenRead(_sensorsFileLocation);
                var res = JsonSerializer.DeserializeAsyncEnumerable<IncomingSensorInput>(_sensorReadStream, cancellationToken: cancellationToken);

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

            if (_sensorReadStream is not null)
            {
                await _sensorReadStream.DisposeAsync();
                _sensorReadStream = null;
            }

            _locker.Dispose();
            _disposed = true;

            GC.SuppressFinalize(this);
        }

        public void Dispose()
        {
            if (_disposed) return;

            _sensorReadStream?.Dispose();
            _sensorReadStream = null;

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
