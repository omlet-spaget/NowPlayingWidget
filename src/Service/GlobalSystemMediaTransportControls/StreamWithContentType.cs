using Windows.Storage.Streams;

namespace NowPlayingWidget.Service.GlobalSystemMediaTransportControls;

internal class StreamWithContentType : IStreamWithContentType
{
    private readonly IRandomAccessStreamWithContentType streamWithContentType;

    public StreamWithContentType(IRandomAccessStreamWithContentType streamWithContentType)
    {
        this.streamWithContentType = streamWithContentType;
    }

    public string ContentType => streamWithContentType.ContentType;

    public Stream Stream => streamWithContentType.AsStreamForRead();

    public void Dispose()
    {
        streamWithContentType.Dispose();
    }
}