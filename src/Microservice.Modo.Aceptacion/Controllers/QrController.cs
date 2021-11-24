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
[Route("qr")]
[ServiceFilter(typeof(GenericActionFilter))]
public class QrController : ControllerBase
{
    private readonly IModoService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    public QrController(IModoService service)
    {
        _service = service;
    }

    /// <summary>
    /// Obtener QR
    /// </summary>
    /// <returns></returns>
    [HttpGet("obtener")]
    [ProducesResponseType(typeof(ObtenerQRResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObtenerQr([BindRequired] [FromQuery] long cuit, [BindRequired] [RegularExpression("[\\d]{22}")] [FromQuery] string cbu, [FromQuery] long? id)
        => Ok(await _service.ObtenerQr(cuit, cbu, id));

    /// <summary>
    /// Generar QR para la cuenta
    /// </summary>
    /// <returns></returns>
    [HttpPost("crear")]
    [ProducesResponseType(typeof(GenerarQrParaLaCuentaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GenerarQrParaLaCuenta(GenerarQrParaLaCuentaRequest request)
        => Ok(await _service.GenerarQrParaLaCuenta(request));

}