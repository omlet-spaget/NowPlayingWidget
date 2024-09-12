using Windows.Storage.Streams;

namespace NowPlayingWidget.Service;

internal interface INowPlayingService
{
    Task<MediaInfo> GetMediaInfo();
    Task<IThumbnail> GetThumbnail();
}
