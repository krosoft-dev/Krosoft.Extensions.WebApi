using System.IO.Compression;
using Krosoft.Extensions.Core.Tools;
using Krosoft.Extensions.WebApi.Interfaces;
using Krosoft.Extensions.WebApi.Models.Results;

namespace Krosoft.Extensions.WebApi.Extensions;

public static class ZipStreamExtensions
{
    /// <param name="compressionLevel">
    /// A passer à <see cref="CompressionLevel.NoCompression" /> pour des fichiers déjà compressés
    /// (images, vidéos, pdf) : le gain de taille est nul et le temps de génération bien moindre.
    /// </param>
    public static ZipStreamResult ToZipStreamResult(this IZipFiles zipFiles,
                                                    CompressionLevel compressionLevel = CompressionLevel.Optimal)
    {
        Guard.IsNotNull(nameof(zipFiles), zipFiles);

        return new ZipStreamResult(zipFiles.FilePathsByEntryName, zipFiles.FileName, compressionLevel);
    }

    /// <inheritdoc cref="ToZipStreamResult(IZipFiles, CompressionLevel)" />
    public static async Task<ZipStreamResult> ToZipStreamResult<T>(this Task<T> task,
                                                                   CompressionLevel compressionLevel = CompressionLevel.Optimal)
        where T : IZipFiles
    {
        var zipFiles = await task;
        return zipFiles.ToZipStreamResult(compressionLevel);
    }
}
