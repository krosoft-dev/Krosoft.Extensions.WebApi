using System.IO.Compression;
using Krosoft.Extensions.WebApi.Models.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace Krosoft.Extensions.WebApi.Tests.Models.Results;

/// <summary>
/// Reproduit les contraintes du flux de réponse HTTP, qu'un MemoryStream ne pose pas : il n'est pas
/// seekable et refuse toute écriture synchrone tant que <see cref="IHttpBodyControlFeature" /> ne l'autorise pas.
/// </summary>
[TestClass]
public class ZipStreamResultVolumeTests
{
    private const int TailleFichier = 30 * 1024 * 1024;

    [TestMethod]
    public async Task ExecuteAsync_GivenNonSeekableBodyAndLargeFiles_WritesCompleteZip()
    {
        var repertoire = Directory.CreateTempSubdirectory("zip-stream-volume");
        var zipPath = Path.Combine(repertoire.FullName, "archive.zip");

        try
        {
            var filePathsByEntryName = new Dictionary<string, string>();
            var random = new Random(42);
            for (var i = 0; i < 3; i++)
            {
                var filePath = Path.Combine(repertoire.FullName, $"file{i}.bin");

                //Contenu incompressible, comme des images déjà compressées.
                var bytes = new byte[TailleFichier];
                random.NextBytes(bytes);
                await File.WriteAllBytesAsync(filePath, bytes);

                filePathsByEntryName.Add($"file{i}.bin", filePath);
            }

            var bodyControlFeature = new BodyControlFeature();

            await using (var fileStream = File.Create(zipPath))
            await using (var body = new NonSeekableStream(fileStream, bodyControlFeature))
            {
                var httpContext = new DefaultHttpContext();
                httpContext.Response.Body = body;
                httpContext.Features.Set<IHttpBodyControlFeature>(bodyControlFeature);

                await new ZipStreamResult(filePathsByEntryName, "archive.zip", CompressionLevel.NoCompression)
                    .ExecuteAsync(httpContext);
            }

            var tailleZip = new FileInfo(zipPath).Length;
            Console.WriteLine($"Taille du zip : {tailleZip} octets pour {TailleFichier * 3} octets de fichiers.");

            Check.That(tailleZip).IsStrictlyGreaterThan(TailleFichier * 3L);

            using var archive = ZipFile.OpenRead(zipPath);
            Check.That(archive.Entries.Select(e => e.FullName)).ContainsExactly("file0.bin", "file1.bin", "file2.bin");
            Check.That(archive.Entries.Select(e => e.Length)).ContainsExactly(TailleFichier, TailleFichier, TailleFichier);
        }
        finally
        {
            repertoire.Delete(true);
        }
    }

    private sealed class BodyControlFeature : IHttpBodyControlFeature
    {
        public bool AllowSynchronousIO { get; set; }
    }

    private sealed class NonSeekableStream : Stream
    {
        private readonly BodyControlFeature _bodyControlFeature;
        private readonly Stream _inner;

        public NonSeekableStream(Stream inner, BodyControlFeature bodyControlFeature)
        {
            _inner = inner;
            _bodyControlFeature = bodyControlFeature;
        }

        public override bool CanRead => false;
        public override bool CanSeek => false;
        public override bool CanWrite => true;
        public override long Length => throw new NotSupportedException();

        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public override void Flush() => _inner.Flush();
        public override Task FlushAsync(CancellationToken cancellationToken) => _inner.FlushAsync(cancellationToken);
        public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();

        public override void Write(byte[] buffer, int offset, int count)
        {
            GuardSynchronousIO();
            _inner.Write(buffer, offset, count);
        }

        public override void Write(ReadOnlySpan<byte> buffer)
        {
            GuardSynchronousIO();
            _inner.Write(buffer);
        }

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            => _inner.WriteAsync(buffer, offset, count, cancellationToken);

        public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
            => _inner.WriteAsync(buffer, cancellationToken);

        //Même comportement que Kestrel lorsque AllowSynchronousIO est désactivé.
        private void GuardSynchronousIO()
        {
            if (!_bodyControlFeature.AllowSynchronousIO)
            {
                throw new InvalidOperationException("Synchronous operations are disallowed. Call WriteAsync or set AllowSynchronousIO to true instead.");
            }
        }

        protected override void Dispose(bool disposing)
        {
            _inner.Flush();
            base.Dispose(disposing);
        }
    }
}
