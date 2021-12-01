using System.Net;
using Modo.Clients.Interfaces;
using Modo.Clients.Models;

namespace Modo.Clients;

public class MerchantClient : BaseClient, IMerchantClient
{
    private readonly HttpClient _client;

    public MerchantClient(HttpClient client)
    {
        _client = client;
    }

    /// <summary>
    /// Obtener comercio por cuit
    /// </summary>
    /// <param name="cuit">Numero de cuit del comercio</param>
    /// <param name="id">Id del comercio en modo opcional</param>
    /// <returns></returns>
    public async Task<ObtenerComercioResponse> ObtenerComercio(long cuit, long? id)
    {
        var url = $"merchant?cuit={cuit}";

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

        return await GetResponse<ObtenerComercioResponse>(response);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<CrearComercioResponse> CrearComercio(CrearComercioRequest request)
    {
        var url = "merchant";

        var response = await _client.PostAsJsonAsync(url, request);

        await HandleError(response);

        return await GetResponse<CrearComercioResponse>(response);
    }
}