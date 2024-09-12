using Windows.Media.Control;

namespace NowPlayingWidget.Service.GlobalSystemMediaTransportControls;

internal class Thumbnail : IThumbnail
{
    private readonly GlobalSystemMediaTransportControlsSessionMediaProperties mediaProperties;

    public Thumbnail(GlobalSystemMediaTransportControlsSessionMediaProperties mediaProperties)
    {
        Artist = mediaProperties.Artist;
        Title = mediaProperties.Title;
        this.mediaProperties = mediaProperties;
    }

    public string Artist { get; private set; }

    public string Title { get; private set; }

    public async Task<IStreamWithContentType?> GetStreamAsync()
    {
        var thumbnail = await mediaProperties.Thumbnail?.OpenReadAsync();
        if (thumbnail is null)
            return null;
        return new StreamWithContentType(thumbnail);
    }
}
