namespace Krosoft.Extensions.Reporting.Csv.Tests.Models;

public record PrixCsvDto
{
    public string? FournisseurNom { get; set; }
    public string? ProduitNom { get; set; }
    public string? VarianteNom { get; set; }
    public decimal Prix { get; set; }
    public string? DeviseCode { get; set; }
}