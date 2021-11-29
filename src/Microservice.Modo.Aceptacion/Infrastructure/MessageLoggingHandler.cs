using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Microservice.Modo.Aceptacion.Infrastructure;

/// <summary>
/// </summary>
public class MessageLoggingHandler : DelegatingHandler
{
    private readonly ILogger<MessageLoggingHandler> _logger;

    public MessageLoggingHandler(ILogger<MessageLoggingHandler> logger)
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            var strRequest = request.Content != null ? await request.Content.ReadAsStringAsync(cancellationToken) : string.Empty;

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("{@request} {strRequest}", request, strRequest);
            }

            var response = await base.SendAsync(request, cancellationToken);

            var strResponse = await response.Content.ReadAsStringAsync(cancellationToken);

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("{@response} {strResponse}", response.Content, strResponse);
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.Accepted)
                    return response;

                _logger.LogWarning("{@request} {strRequest}", request, strRequest);
                _logger.LogWarning("{@response} {strResponse}", response.Content, strResponse);
            }

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            throw;
        }
    }
}