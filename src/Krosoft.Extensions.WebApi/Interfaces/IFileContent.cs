namespace Krosoft.Extensions.WebApi.Interfaces;

/// <summary>
/// Contenu binaire servable en tant que fichier HTTP (avatar, document, etc.).
/// Implémenter cette interface sur un DTO le rend éligible à <c>ToFileResult()</c>.
/// </summary>
public interface IFileContent
{
    byte[] Data { get; }
    string ContentType { get; }
}
