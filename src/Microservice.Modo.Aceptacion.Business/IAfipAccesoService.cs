using System.Threading.Tasks;
using Microservice.PagosAFIP.Business.Model;

namespace Microservice.Modo.Aceptacion.Business;

public interface IAfipAccesoService
{
    Task<GenerateTokenResponse> ObtenerTokenAsync();
}