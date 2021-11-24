using Modo.Clients.Models;

namespace Modo.Clients.Interfaces;

public interface IQrClient
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<GenerarQrParaLaCuentaResponse> GenerarQrParaLaCuenta(GenerarQrParaLaCuentaRequest request);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cuit"></param>
    /// <param name="cbu"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ObtenerQRResponse> ObtenerQr(long cuit, string cbu, long? id);
}