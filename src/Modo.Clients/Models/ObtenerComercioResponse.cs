using Newtonsoft.Json;

namespace Modo.Clients.Models;

public class ObtenerComercioResponse
{
    /// <summary>
    /// UUID del commercio
    /// </summary>
    [JsonProperty("uuid")]
    public string UUID { get; set; } = null!;

    /// <summary>
    /// cuit del commercio
    /// </summary>
    [JsonProperty("cuit")]
    public string CUIT { get; set; } = null!;
}