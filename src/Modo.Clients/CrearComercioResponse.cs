﻿using Newtonsoft.Json;

namespace Modo.Clients;

public class CrearComercioResponse
{
    /// <summary>
    /// UUID del commercio
    /// </summary>
    [JsonProperty("uuid")]
    public string UUID { get; set; } = null!;
}