using System.Net;
using System.Net.Http.Json;
using Modo.Clients.Interfaces;
using Modo.Clients.Models;

namespace Modo.Clients;

public class QrClient : BaseClient, IQrClient
{
    private readonly HttpClient _client;

    public QrClient(HttpClient client)
    {
        _client = client;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<GenerarQrParaLaCuentaResponse> GenerarQrParaLaCuenta(GenerarQrParaLaCuentaRequest request)
    {
        var url = "qr";

        var response = await _client.PostAsJsonAsync(url, request);

        await HandleError(response);

        return await GetResponse<GenerarQrParaLaCuentaResponse>(response);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cuit"></param>
    /// <param name="cbu"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="ApiException"></exception>
    public async Task<ObtenerQRResponse> ObtenerQr(long cuit, string cbu, long? id)
    {
        var url = $"qr?cuit={cuit}&cbu={cbu}";

        if (id.HasValue)
            url = $"{url}&id={id}";

        var response = await _client.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            var notfound = await response.Content.ReadFromJsonAsync<Error404>();

            if (notfound == null)
                throw new ApiException();

            throw new ApiException(notfound.StatusCode, notfound.Error ?? "", notfound.Message);
        }

        await HandleError(response);

        return await GetResponse<ObtenerQRResponse>(response);

    }
}