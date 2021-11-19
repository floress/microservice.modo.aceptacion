using System.Threading.Tasks;
using Microservice.Modo.Aceptacion;
using Microservice.Modo.Aceptacion.Business;
using Microservice.PagosAFIP.Business;
using Microservice.PagosAFIP.Business.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Modo.Aceptacion.Controllers;

/// <summary>
/// 
/// </summary>
[ApiController]
[Route("[controller]")]
[ServiceFilter(typeof(GenericActionFilter))]
public class AccesoController : ControllerBase
{
    private readonly IAfipAccesoService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    public AccesoController(IAfipAccesoService service)
    {
        _service = service;
    }

    /// <summary>
    /// Obtener token de acceso
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(GenerateTokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObtenerToken()
        => Ok(await _service.ObtenerTokenAsync());

}