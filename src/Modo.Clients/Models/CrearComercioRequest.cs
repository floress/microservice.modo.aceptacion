using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Modo.Clients.Models;

public class CrearComercioRequest
{
    /// <summary>
    /// CUIT
    /// </summary>
    [JsonProperty("cuit")]
    [RegularExpression("[\\d]{11}", ErrorMessage = "CUIT inválido")]
    public string Cuit { get; set; } = null!;

    /// <summary>
    /// email
    /// </summary>
    [JsonProperty("email")]
    [EmailAddress]
    [StringLength(250)]
    public string Email { get; set; } = null!;

    /// <summary>
    /// Nombre fantasia / Nombre
    /// </summary>
    [JsonProperty("softDescriptor")]
    public string SoftDescriptor { get; set; } = null!;

    /// <summary>
    /// Razón social
    /// </summary>
    [JsonProperty("businessName")]
    public string BusinessName { get; set; } = null!;

    [JsonProperty("segment")]
    [JsonConverter(typeof(StringEnumConverter))]
    public SegmentEnum Segment { get; set; }

    /// <summary>
    /// Codigo AFIP
    /// </summary>
    [JsonProperty("activity")]
    public long Activity { get; set; }

    /// <summary>
    /// Identifica si esta exceptuado de iva
    /// </summary>
    [JsonProperty("isExceptIva")]
    public bool? IsExceptIva { get; set; }

    /// <summary>
    /// Condiciones fiscales
    /// </summary>
    [JsonProperty("taxsCondition")]
    public List<TaxsCondition>? TaxsCondition { get; set; }

    /// <summary>
    /// True si es persona juridica, False persona fisica
    /// </summary>
    [JsonProperty("isLegalPerson")]
    public bool IsLegalPerson { get; set; }

    /// <summary>
    /// M masculino, F femenino, X otro. Aplica solo para persona fisica.
    /// </summary>
    [JsonProperty("gender")]
    public GenderEnum Gender { get; set; }

    /// <summary>
    /// Sucursal
    /// </summary>
    [JsonProperty("branch")]
    public Branch Branch { get; set; } = null!;
}