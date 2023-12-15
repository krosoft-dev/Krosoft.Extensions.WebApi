using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Core.Tools;
using Krosoft.Extensions.Pdf.Interfaces;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;

namespace Krosoft.Extensions.Pdf.Services;

public class PdfService : IPdfService
{
    public Stream Merge(params Stream[] streams)
    {
        Guard.IsNotNull(nameof(streams), streams);
        var output = new MemoryStream();

        if (streams.Any())
        {
            try
            {
                using (var pdf = new PdfDocument())
                {
                    foreach (var stream in streams)
                    {
                        using (var from = PdfReader.Open(stream, PdfDocumentOpenMode.Import))
                        {
                            foreach (var page in from.Pages)
                            {
                                pdf.AddPage(page);
                            }
                        }
                    }

                    pdf.Save(output);
                }
            }
            finally
            {
                foreach (var stream in streams)
                {
                    stream.Dispose();
                }
            }
        }

        return output;
    }

    public byte[] Merge(params byte[][] files)
    {
        Guard.IsNotNull(nameof(files), files);

        var streams = files.ToStreams();
        var stream = Merge(streams.ToArray());
        return stream.ToByte();
    }
}