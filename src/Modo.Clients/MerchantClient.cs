using System.Net;
using System.Net.Http.Json;

namespace Modo.Clients;

public interface IMerchantClient
{
    /// <summary>
    /// Obtener comercio por cuit
    /// </summary>
    /// <param name="cuit">Numero de cuit del comercio</param>
    /// <param name="id">Id del comercio en modo opcional</param>
    /// <returns></returns>
    Task<ObtenerComercioResponse> ObtenerComercio(long cuit, long? id);
    Task<CrearComercioResponse> CrearComercio(CrearComercioRequest request);
}

public class MerchantClient : IMerchantClient
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

            throw new ApiException(notfound.StatusCode, notfound.Error, notfound.Message);
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

    private async Task HandleError(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
            return;

        var error = await response.Content.ReadFromJsonAsync<Error>();

        if (error != null)
            throw new ApiException(error.Code, error.Message);

        throw new ApiException();
    }

    private async Task<T> GetResponse<T>(HttpResponseMessage response)
    {
        var obj = await response.Content.ReadFromJsonAsync<T>();

        if (obj == null)
            throw new ApiException();

        return obj;
    }
}