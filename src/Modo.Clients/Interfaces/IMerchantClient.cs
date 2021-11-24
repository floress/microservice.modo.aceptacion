using Modo.Clients.Models;

namespace Modo.Clients.Interfaces;

public interface IMerchantClient
{
    /// <summary>
    /// Obtener comercio por cuit
    /// </summary>
    /// <param name="cuit">Numero de cuit del comercio</param>
    /// <param name="id">Id del comercio en modo opcional</param>
    /// <returns></returns>
    Task<ObtenerComercioResponse> ObtenerComercio(long cuit, long? id);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<CrearComercioResponse> CrearComercio(CrearComercioRequest request);
}