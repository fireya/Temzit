using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;

namespace Temzit.Api
{
    public class TemzitReader : ITemzitReader
    {
        private readonly IOptions<TemzitOptions> _temzitOptions;
        private readonly ILogger<TemzitReader> _logger;
        private readonly AsyncRetryPolicy _policy;

        public TemzitReader(IOptions<TemzitOptions> temzitOptions, ILogger<TemzitReader> logger)
        {
            _temzitOptions = temzitOptions;
            _logger = logger;
            _policy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(new[]
                    {TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(30)});
        }

        public async Task<TemzitActualState> ReadState()
        {
            return await _policy.ExecuteAsync(async () => await ReadStateInternal());
        }

        private async Task<TemzitActualState> ReadStateInternal()
        {
            try
            {
                _logger.LogDebug("Reading state");
                var port = 333;
                using var client = new TcpClient(_temzitOptions.Value.Host, port);
                byte[] syncRequest = {0x30, 0x00};
                await using var stream = client.GetStream();
                await stream.WriteAsync(syncRequest, 0, syncRequest.Length);
                var stateResponse = new byte[64];
                var bytes = await stream.ReadAsync(stateResponse, 0, stateResponse.Length);
                if (stateResponse[0] == 0x01 && stateResponse[1] == 0x00)
                {
                    var state = new TemzitActualState(stateResponse[2..]);

                    _logger.LogDebug("State ready {state}", state);
                    return state;

                }

                _logger.LogError("Invalid answer");
                throw new Exception("Wrong answer");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "error reading state");
                throw;
            }
        }
    }
}