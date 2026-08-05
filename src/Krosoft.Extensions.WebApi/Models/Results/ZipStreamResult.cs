using System.IO.Compression;
using System.Net.Mime;
using Krosoft.Extensions.Core.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Net.Http.Headers;

namespace Krosoft.Extensions.WebApi.Models.Results;

/// <summary>
/// Ecrit une archive zip directement dans le flux de réponse, sans jamais la matérialiser en mémoire :
/// l'empreinte mémoire reste celle d'un seul fichier, quelle que soit la taille totale de l'archive.
/// </summary>
public class ZipStreamResult : IResult
{
    private const int BufferSize = 81920;

    private readonly CompressionLevel _compressionLevel;
    private readonly IReadOnlyDictionary<string, string> _filePathsByEntryName;

    /// <param name="filePathsByEntryName">Chemin physique du fichier source, par nom d'entrée dans l'archive.</param>
    /// <param name="fileName">Nom de l'archive proposé au téléchargement.</param>
    /// <param name="compressionLevel">
    /// A passer à <see cref="CompressionLevel.NoCompression" /> pour des fichiers déjà compressés
    /// (images, vidéos, pdf) : le gain de taille est nul et le temps de génération bien moindre.
    /// </param>
    public ZipStreamResult(IReadOnlyDictionary<string, string> filePathsByEntryName,
                           string fileName,
                           CompressionLevel compressionLevel = CompressionLevel.Optimal)
    {
        Guard.IsNotNull(nameof(filePathsByEntryName), filePathsByEntryName);
        Guard.IsNotNullOrWhiteSpace(nameof(fileName), fileName);

        _filePathsByEntryName = filePathsByEntryName;
        _compressionLevel = compressionLevel;
        FileName = fileName;
    }

    public string FileName { get; }

    public async Task ExecuteAsync(HttpContext httpContext)
    {
        Guard.IsNotNull(nameof(httpContext), httpContext);

        var response = httpContext.Response;
        response.ContentType = MediaTypeNames.Application.Zip;

        var contentDisposition = new ContentDispositionHeaderValue("attachment");
        contentDisposition.SetHttpFileName(FileName);
        response.Headers.ContentDisposition = contentDisposition.ToString();

        var cancellationToken = httpContext.RequestAborted;

        //ZipArchive ferme ses entrées de façon synchrone, même via DisposeAsync : sans cela le serveur
        //refuse l'écriture du descripteur de données et de l'index, et l'archive produite est illisible.
        var bodyControlFeature = httpContext.Features.Get<IHttpBodyControlFeature>();
        if (bodyControlFeature != null)
        {
            bodyControlFeature.AllowSynchronousIO = true;
        }

        //Le volume, lui, transite par les API asynchrones : aucun thread n'est bloqué sur la copie.
        var archive = await ZipArchive.CreateAsync(response.Body, ZipArchiveMode.Create, true, null, cancellationToken);
        await using (archive)
        {
            foreach (var filePathByEntryName in _filePathsByEntryName)
            {
                if (!File.Exists(filePathByEntryName.Value))
                {
                    continue;
                }

                //Le séparateur d'une entrée d'archive est toujours '/', quel que soit l'OS.
                var entryName = filePathByEntryName.Key.Replace('\\', '/');

                var entry = archive.CreateEntry(entryName, _compressionLevel);
                await using var entryStream = await entry.OpenAsync(cancellationToken);
                await using var fileStream = new FileStream(filePathByEntryName.Value,
                                                            FileMode.Open,
                                                            FileAccess.Read,
                                                            FileShare.Read,
                                                            BufferSize,
                                                            true);
                await fileStream.CopyToAsync(entryStream, cancellationToken);
            }
        }
    }
}
