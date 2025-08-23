﻿using Newtonsoft.Json;

namespace Krosoft.Extensions.Samples.Library.Models;

public record TodoHttp
{
    [JsonProperty("userId")]
    public int UserId { get; set; }

    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("title")]
    public string? Title { get; set; }

    [JsonProperty("completed")]
    public bool Completed { get; set; }
}