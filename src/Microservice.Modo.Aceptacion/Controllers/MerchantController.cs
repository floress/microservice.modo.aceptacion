using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microservice.Modo.Aceptacion.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Modo.Clients.Models;

namespace Microservice.Modo.Aceptacion.Controllers;

/// <summary>
/// 
/// </summary>
[ApiController]
[Route("merchant")]
//[ServiceFilter(typeof(GenericActionFilter))]
public class MerchantController : ControllerBase
{
    private readonly IModoService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    public MerchantController(IModoService service)
    {
        _service = service;
    }

    /// <summary>
    /// Obtener comercio por cuit
    /// </summary>
    /// <returns></returns>
    [HttpGet("obtener")]
    [ProducesResponseType(typeof(ObtenerComercioResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObtenerComercio([BindRequired] [FromQuery] [Range(20000000000, 39999999999)] long cuit, [FromQuery] long? id)
        => Ok(await _service.ObtenerComercio(cuit, id));

    /// <summary>
    /// Crear comercio
    /// </summary>
    /// <returns></returns>
    [HttpPost("crear")]
    [ProducesResponseType(typeof(CrearComercioResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CrearComercio(CrearComercioRequest request)
        => Ok(await _service.CrearComercio(request));

}