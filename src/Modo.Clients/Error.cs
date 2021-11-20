﻿using Newtonsoft.Json;

namespace Modo.Clients;

public class Error
{
    [JsonProperty("code")]
    public int Code { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; } = null!;
}