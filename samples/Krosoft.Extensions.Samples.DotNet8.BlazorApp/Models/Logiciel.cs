using System.Text.Json.Serialization;

namespace Krosoft.Extensions.Samples.DotNet8.BlazorApp.Models;

public class Logiciel
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("nom")]
    public string? Nom { get; set; }

    [JsonPropertyName("categorie")]
    public string? Categorie { get; set; }

    [JsonPropertyName("statutCode")]
    public int? StatutCode { get; set; }

    [JsonPropertyName("dateCreation")]
    public DateTime? DateCreation { get; set; }
}