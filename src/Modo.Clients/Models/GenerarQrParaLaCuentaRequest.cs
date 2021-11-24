using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Modo.Clients.Models;

public class GenerarQrParaLaCuentaRequest
{
    /// <summary>
    /// Id del comercio (uuid) devuelto en la creacion este parametro no es necesario si se informa el cuit
    /// </summary>
    [JsonProperty("merchantId")]
    public string? MerchantId { get; set; }

    /// <summary>
    /// CUIT del comercio
    /// </summary>
    [JsonProperty("cuit")]
    public string CUIT { get; set; } = null!;

    [JsonProperty("cbu")]
    public string CBU { get; set; } = null!;

    /// <summary>
    /// Tipo de cuenta (en esta etapa son current)
    /// </summary>
    [JsonProperty("accountType")]
    [JsonConverter(typeof(StringEnumConverter))]
    public AccountTypeEnum AccountType { get; set; }

    [JsonProperty("imageLogo")]
    public ImageLogoClass? ImageLogo { get; set; }

    public class ImageLogoClass
    {
        /// <summary>
        /// Formato de la imagen subida por ejemplo image/jpeg ,image/png,image/svg
        /// </summary>
        [JsonProperty("contentType")]
        public string ContentType { get; set; } = null!;

        /// <summary>
        /// Base64 de la imagen subida por el comercio 100X100 PX y maximo de 10KB
        /// </summary>
        [JsonProperty("base64")]
        [StringLength(10 * 1024)]
        public string Base64 { get; set; } = null!;

    }
}