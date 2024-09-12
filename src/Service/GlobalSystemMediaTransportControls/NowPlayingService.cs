using Windows.Media.Control;
using Windows.Storage.Streams;

namespace NowPlayingWidget.Service.GlobalSystemMediaTransportControls;

internal class NowPlayingService : INowPlayingService
{
    private readonly TimeProvider timeProvider;
    private GlobalSystemMediaTransportControlsSessionManager? sessionManager;

    public NowPlayingService(TimeProvider timeProvider)
    {
        this.timeProvider = timeProvider;
    }

    public static async Task<NowPlayingService> Create(TimeProvider timeProvider)
    {
        var service = new NowPlayingService(timeProvider);
        await service.GetSessionManager();
        return service;
    }

    public async Task<GlobalSystemMediaTransportControlsSessionManager> GetSessionManager()
    {
        return sessionManager ??= (await CreateSessionManager());
    }

    private async Task<GlobalSystemMediaTransportControlsSessionManager> CreateSessionManager()
    {
        var sessionManager = await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
        return sessionManager;
    }

    static GlobalSystemMediaTransportControlsSession GetBestSession(GlobalSystemMediaTransportControlsSessionManager sessionManager)
    {
        var sessions = sessionManager.GetSessions().ToArray();
        var session = sessions.Where(s => s.GetPlaybackInfo().PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing).OrderBy(s => s.GetTimelineProperties().EndTime).FirstOrDefault();
        if (session is null)
            session = sessionManager.GetCurrentSession();
        return session;
    }

    public async Task<MediaInfo> GetMediaInfo()
    {
        var gsmtcsm = await GetSessionManager();
        var currentSession = GetBestSession(gsmtcsm);
        if (currentSession is null)
        {
            return new MediaInfo("", "", "", false, 0, 1);
        }

        var playbackInfo = currentSession.GetPlaybackInfo();
        var timeline = currentSession.GetTimelineProperties();
        var mediaProperties = await currentSession.TryGetMediaPropertiesAsync();

        var isPlaying = playbackInfo.PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing;

        var timelinePosition = timeline.Position.TotalSeconds + (isPlaying ? (timeProvider.GetLocalNow() - timeline.LastUpdatedTime).TotalSeconds : 0);
        var timelineTotal = (timeline.EndTime - timeline.StartTime).TotalSeconds;

        return new MediaInfo(currentSession.SourceAppUserModelId, mediaProperties.Artist, mediaProperties.Title, isPlaying, timelinePosition, timelineTotal);
    }

    public async Task<IThumbnail> GetThumbnail()
    {
        var gsmtcsm = await GetSessionManager();
        var session = GetBestSession(gsmtcsm);
        var mediaProperties = await session?.TryGetMediaPropertiesAsync();
        return new Thumbnail(mediaProperties);
    }
}
