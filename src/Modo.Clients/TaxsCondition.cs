using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Modo.Clients;

public class TaxsCondition
{
    /// <summary>
    /// Número de jurisdicción (901 a 924) sobre la cual se indican datos de condición fiscal del comercio. En caso de tratarse de condición fiscal ante AFIP, se debe indicar "AFIP"
    /// </summary>
    [JsonProperty("jurisdiction")]
    public string Jurisdiction { get; set; } = null!;

    /// <summary>
    /// Tipo de condición fiscal
    /// </summary>
    [JsonProperty("type")]
    [JsonConverter(typeof(StringEnumConverter))]
    public TipoCondicionFiscalEnum Type { get; set; }

    /// <summary>
    /// Verdadero o falso
    /// </summary>
    [JsonProperty("value")]
    public bool Value { get; set; }

    /// <summary>
    /// Fecha de fin de vigencia de la condición fiscal informada. en formato YYYY-MM-DD
    /// </summary>
    [JsonProperty("dueDate")]
    public string? DueDate { get; set; }
}