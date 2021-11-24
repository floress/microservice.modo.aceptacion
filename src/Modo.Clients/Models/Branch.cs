using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Modo.Clients.Models;

public class Branch : IValidatableObject
{
    /// <summary>
    /// Codigo postal
    /// </summary>
    [JsonProperty("zipCode")]
    public string ZipCode { get; set; } = null!;

    /// <summary>
    /// Localidad
    /// </summary>
    [JsonProperty("locality")]
    public string Locality { get; set; } = null!;

    [JsonProperty("street")]
    public string? Street { get; set; }

    [JsonProperty("streetNumber")]
    public string? StreetNumber { get; set; }

    /// <summary>
    /// Codigo de provincia: Enum: 901 902 903 904 905 906 907 908 909 911 912 913 914 915 916 917 918 919 920 921 924
    /// </summary>
    [JsonProperty("provinceCode")]
    public int? ProvinceCode { get; set; }

    /// <summary>
    /// Datos adicionales del domicilio ej 3er piso
    /// </summary>
    [JsonProperty("reference")]
    public string? Reference { get; set; }

    [JsonProperty("latitude")]
    public double? Latitude { get; set; }

    [JsonProperty("longitude")]
    public double? Longitude { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var provincias = new[]
        {
            901,
            902,
            903,
            904,
            905,
            906,
            907,
            908,
            909,
            910,
            911,
            912,
            913,
            914,
            915,
            916,
            917,
            918,
            919,
            920,
            921,
            922,
            923,
            924
        };

        if (ProvinceCode.HasValue && !provincias.Contains(ProvinceCode.Value))
            yield return new ValidationResult($"{nameof(ProvinceCode)} is invalid");
    }
}