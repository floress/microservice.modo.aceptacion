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
}