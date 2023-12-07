using Krosoft.Extensions.Samples.Library.Models.Enums;

namespace Krosoft.Extensions.Samples.Library.Models.Entities;

public class Logiciel
{
    public Guid Id { get; set; }
    public string? Nom { get; set; }
    public string? Categorie { get; set; }
    public StatutCode StatutCode { get; set; }
    public DateTime DateCreation { get; set; }
}