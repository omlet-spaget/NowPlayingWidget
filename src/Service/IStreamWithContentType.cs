namespace NowPlayingWidget.Service;

public interface IStreamWithContentType : IDisposable
{
    public string ContentType { get; }
    public Stream Stream { get; }
}