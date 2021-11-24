using Newtonsoft.Json;

namespace Modo.Clients.Models;

public class ObtenerQRResponse
{
    /// <summary>
    /// Identificador del QR dentro de MODO
    /// </summary>
    [JsonProperty("uuid")]
    public string UUID { get; set; } = null!;

    /// <summary>
    /// Url de la imagen del qr temporal
    /// </summary>
    [JsonProperty("urlQR")]
    public string UrlQR { get; set; } = null!;

    /// <summary>
    /// Signed URL de la imagen
    /// </summary>
    [JsonProperty("base64")]
    public string Base64 { get; set; } = null!;
}