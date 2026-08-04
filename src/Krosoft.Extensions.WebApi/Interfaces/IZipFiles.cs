namespace Krosoft.Extensions.WebApi.Interfaces;

/// <summary>
/// Décrit une archive zip par ses fichiers sources, sans la construire :
/// à passer à <see cref="Krosoft.Extensions.WebApi.Extensions.ZipStreamExtensions.ToZipStreamResult(IZipFiles, System.IO.Compression.CompressionLevel)" />.
/// </summary>
public interface IZipFiles
{
    /// <summary>
    /// Nom de l'archive proposé au téléchargement.
    /// </summary>
    string FileName { get; }

    /// <summary>
    /// Chemin physique du fichier source, par nom d'entrée dans l'archive.
    /// </summary>
    IReadOnlyDictionary<string, string> FilePathsByEntryName { get; }
}
