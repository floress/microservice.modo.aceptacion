using System.Threading.Tasks;
using Modo.Clients.Models;

namespace Microservice.Modo.Aceptacion.Business;

public interface IModoService
{
    Task<ObtenerComercioResponse> ObtenerComercio(long cuit, long? id);

    Task<CrearComercioResponse> CrearComercio(CrearComercioRequest request);

    Task<GenerarQrParaLaCuentaResponse> GenerarQrParaLaCuenta(GenerarQrParaLaCuentaRequest request);

    Task<ObtenerQRResponse> ObtenerQr(long cuit, string cbu, long? id);
}