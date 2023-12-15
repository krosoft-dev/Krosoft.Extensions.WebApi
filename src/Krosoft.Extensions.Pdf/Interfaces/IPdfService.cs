namespace Krosoft.Extensions.Pdf.Interfaces;

public interface IPdfService
{
    Stream Merge(params Stream[] streams);
    byte[] Merge(params byte[][] files);
}