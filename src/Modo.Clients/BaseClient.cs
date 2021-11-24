using System.Net.Http.Json;
using Modo.Clients.Models;

namespace Modo.Clients;

public abstract class BaseClient
{
    protected async Task HandleError(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
            return;

        var error = await response.Content.ReadFromJsonAsync<Error>();

        if (error != null)
            throw new ApiException(error.Code, error.Message);

        throw new ApiException();
    }

    protected async Task<T> GetResponse<T>(HttpResponseMessage response)
    {
        var obj = await response.Content.ReadFromJsonAsync<T>();

        if (obj == null)
            throw new ApiException();

        return obj;
    }
}