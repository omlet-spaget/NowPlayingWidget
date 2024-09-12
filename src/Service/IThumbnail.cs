namespace NowPlayingWidget.Service;

public interface IThumbnail
{
    public string Artist { get; }
    public string Title { get; }
    public Task<IStreamWithContentType?> GetStreamAsync();
}