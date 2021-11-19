using System;
using System.Globalization;
using Newtonsoft.Json;

namespace Microservice.Modo.Aceptacion.Infrastructure;

public class CustomDatetimeConverter : JsonConverter
{
    public string Format { get; }

    // ReSharper disable once UnusedMember.Global
    public CustomDatetimeConverter() : this("yyyy-MM-dd")
    {

    }

    public CustomDatetimeConverter(string format)
    {
        Format = format;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        if (value is DateTime date)
            writer.WriteValue(date.ToString(Format));
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var value = reader.Value;
        if (value == null)
        {
            if (objectType == typeof(DateTime))
                return new DateTime();

            return null;
        }

        var str = $"{value}";
        if (DateTime.TryParseExact(str, Format, CultureInfo.CurrentCulture, DateTimeStyles.None, out var date))
            return (DateTime?)date;

        throw new FormatException($"Error en el formato de fecha, debe ser {Format}");
    }

    public override bool CanConvert(Type objectType)
    {
        if (objectType == typeof(DateTime)) return true;
        if (objectType == typeof(DateTime?)) return true;
        return false;
    }
}