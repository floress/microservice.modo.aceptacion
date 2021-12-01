using System.Collections.Generic;
using Microservice.Modo.Aceptacion.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modo.Clients.Models;

namespace Microservice.Modo.Aceptacion.Controllers;

/// <summary>
/// 
/// </summary>
[ApiController]
[Route("parameters")]
public class ParametersController : ControllerBase
{
    private readonly IModoService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    public ParametersController(IModoService service)
    {
        _service = service;
    }

    /// <summary>
    /// Obtener listado de provincias
    /// </summary>
    /// <returns></returns>
    [HttpGet("provincias")]
    [ProducesResponseType(typeof(List<KeyValue>), StatusCodes.Status200OK)]
    public IActionResult GetProvincias()
        => Ok(_service.GetProvincias());

    /// <summary>
    /// Obtener listado de segmentos
    /// </summary>
    /// <returns></returns>
    [HttpGet("segmentos")]
    [ProducesResponseType(typeof(List<KeyValue>), StatusCodes.Status200OK)]
    public IActionResult GetSegmentos()
        => Ok(_service.GetSegmentos());

}