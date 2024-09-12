namespace NowPlayingWidget.Service;

internal record MediaInfo(string Source, string Artist, string Title, bool Playing, double TimelinePosition, double TimelineTotal);
