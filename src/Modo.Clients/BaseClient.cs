using Modo.Clients.Models;

namespace Modo.Clients;

public abstract class BaseClient
{
    protected async Task HandleError(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
            return;

        var error = await response.Content.ReadFromJsonAsync<Error>();

        if (error == null)
            throw new ApiException();

        string msg;

        if (error.Message is string[] errors)
            msg = string.Join("\n", errors);
        else
            msg = $"{error.Message ?? error.ErrorDescripcion}";

        throw new ApiException(error.Code ?? error.StatusCode ?? -1, msg);

    }

    protected async Task<T> GetResponse<T>(HttpResponseMessage response)
    {
        var obj = await response.Content.ReadFromJsonAsync<T>();

        if (obj == null)
            throw new ApiException();

        return obj;
    }
}