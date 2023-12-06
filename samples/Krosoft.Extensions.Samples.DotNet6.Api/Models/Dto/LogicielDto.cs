using Krosoft.Extensions.Samples.Library.Models.Enums;

namespace Krosoft.Extensions.Samples.DotNet6.Api.Models.Dto;

public class LogicielDto
{
    public string? Nom { get; set; }
    public string? Categorie { get; set; }
    public StatutCode StatutCode { get; set; }
}