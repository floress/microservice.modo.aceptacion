using Newtonsoft.Json;

namespace Modo.Clients.Models;

public class Error
{
    [JsonProperty("code")]
    public int? Code { get; set; }

    [JsonProperty("message")]
    public object? Message { get; set; }

    [JsonProperty("error")]
    public object? ErrorDescripcion { get; set; }

    [JsonProperty("statusCode")]
    public int? StatusCode { get; set; }
}