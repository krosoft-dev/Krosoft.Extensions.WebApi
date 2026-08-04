using Krosoft.Extensions.WebApi.Interfaces;

namespace Krosoft.Extensions.WebApi.Models;

/// <inheritdoc cref="IZipFiles" />
public record ZipFiles(string FileName, IReadOnlyDictionary<string, string> FilePathsByEntryName) : IZipFiles;
