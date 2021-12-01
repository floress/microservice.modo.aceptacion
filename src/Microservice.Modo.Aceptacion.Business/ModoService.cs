using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Modo.Clients.Interfaces;
using Modo.Clients.Models;

namespace Microservice.Modo.Aceptacion.Business;

public class ModoService : IModoService
{
    private readonly IMerchantClient _merchantClient;
    private readonly IQrClient _qrClient;

    public ModoService(IMerchantClient merchantClient, 
        IQrClient qrClient)
    {
        _merchantClient = merchantClient;
        _qrClient = qrClient;
    }

    public async Task<ObtenerComercioResponse> ObtenerComercio(long cuit, long? id) 
        => await _merchantClient.ObtenerComercio(cuit, id);

    public async Task<CrearComercioResponse> CrearComercio(CrearComercioRequest request) 
        => await _merchantClient.CrearComercio(request);

    public async Task<GenerarQrParaLaCuentaResponse> GenerarQrParaLaCuenta(GenerarQrParaLaCuentaRequest request)
        => await _qrClient.GenerarQrParaLaCuenta(request);

    public async Task<ObtenerQRResponse> ObtenerQr(long cuit, string cbu, long? id)
        => await _qrClient.ObtenerQr(cuit, cbu, id);

    public IEnumerable<KeyValue> GetProvincias()
    {
        yield return new KeyValue(901, "Ciudad de Buenos Aires");
        yield return new KeyValue(902, "Provincia de Buenos Aires");
        yield return new KeyValue(903, "Catamarca");
        yield return new KeyValue(904, "Córdoba");
        yield return new KeyValue(905, "Corrientes");
        yield return new KeyValue(906, "Chaco");
        yield return new KeyValue(907, "Chubut");
        yield return new KeyValue(908, "Entre Ríos");
        yield return new KeyValue(909, "Formosa");
        yield return new KeyValue(910, "Jujuy");
        yield return new KeyValue(911, "La Pampa");
        yield return new KeyValue(912, "La Rioja");
        yield return new KeyValue(913, "Mendoza");
        yield return new KeyValue(914, "Misiones");
        yield return new KeyValue(915, "Neuquén");
        yield return new KeyValue(916, "Río Negro");
        yield return new KeyValue(917, "Salta");
        yield return new KeyValue(918, "San Juan");
        yield return new KeyValue(919, "San Luis");
        yield return new KeyValue(920, "Santa Cruz");
        yield return new KeyValue(921, "Santa Fe");
        yield return new KeyValue(922, "Santiago del Estero");
        yield return new KeyValue(923, "Tierra del Fuego");
        yield return new KeyValue(924, "Tucumán");
    }

    public IEnumerable<KeyValue> GetSegmentos()
    {
        foreach (SegmentEnum segment in Enum.GetValues(typeof(SegmentEnum)))
            yield return new KeyValue((int) segment, Enum.GetName(segment) ?? String.Empty);
    }
}