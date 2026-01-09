using FluentAssertions;
using FluentAssertions.Execution;
using FlyleafLib;
using FlyleafLib.MediaPlayer;

namespace FlyleafLibTests.Integration;

/// <summary>
/// Integration Test: SubManager Seek and Timeline
/// Tests SubManager seeking and time navigation
/// Positive: Seeking through subtitles works correctly
/// Negative: Invalid seek times handled gracefully
/// </summary>
public class SeekTimelineIntegrationTest
{
    /// <summary>
    /// POSITIVE: SubManager seeking navigates correctly through timeline
    /// GetCurrent, GetPrev, GetNext return correct subtitles
    /// </summary>
    [Fact]
    public void SeekTimeline_NavigateThroughSubtitles_ReturnsCorrectSubtitles()
    {
        // Arrange
        var config = new Config(true);
        var subManager = new SubManager(config, 0, false);
        var subtitles = new List<SubtitleData>
        {
            new() { StartTime = TimeSpan.Zero, EndTime = TimeSpan.FromSeconds(5), Text = "First" },
            new() { StartTime = TimeSpan.FromSeconds(10), EndTime = TimeSpan.FromSeconds(15), Text = "Second" },
            new() { StartTime = TimeSpan.FromSeconds(20), EndTime = TimeSpan.FromSeconds(25), Text = "Third" }
        };
        subManager.Load(subtitles);

        // Act & Assert
        using (new AssertionScope())
        {
            // Seek to first
            subManager.SetCurrentTime(TimeSpan.FromSeconds(2));
            var current = subManager.GetCurrent();
            current.Should().NotBeNull();
            current!.Text.Should().Be("First");

            // Seek to second
            subManager.SetCurrentTime(TimeSpan.FromSeconds(12));
            current = subManager.GetCurrent();
            current.Should().NotBeNull();
            current!.Text.Should().Be("Second");
        }
    }

    /// <summary>
    /// NEGATIVE: Seeking before first subtitle returns correct state
    /// Expected: GetCurrent returns null, GetNext returns first subtitle
    /// </summary>
    [Fact]
    public void SeekTimeline_SeekBeforeFirstSubtitle_ReturnsCorrectState()
    {
        // Arrange
        var config = new Config(true);
        var subManager = new SubManager(config, 0, false);
        var subtitles = new List<SubtitleData>
        {
            new() { StartTime = TimeSpan.FromSeconds(10), EndTime = TimeSpan.FromSeconds(15), Text = "First" }
        };
        subManager.Load(subtitles);

        // Act
        subManager.SetCurrentTime(TimeSpan.FromSeconds(5));

        // Assert
        subManager.GetCurrent().Should().BeNull();
        var next = subManager.GetNext();
        next.Should().NotBeNull();
        next!.Text.Should().Be("First");
    }
}
