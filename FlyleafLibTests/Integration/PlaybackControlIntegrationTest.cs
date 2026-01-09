using FluentAssertions;
using FluentAssertions.Execution;
using FlyleafLib;
using FlyleafLib.MediaPlayer;

namespace FlyleafLibTests.Integration;

/// <summary>
/// Integration Test: Subtitle Manager Playback Navigation
/// Tests SubManager state machine and navigation through subtitles
/// Positive: State transitions work correctly (First → Around → Showing → Last)
/// Negative: Invalid operations handled gracefully
/// </summary>
public class PlaybackControlIntegrationTest
{
    /// <summary>
    /// POSITIVE: SubManager state transitions work correctly
    /// Tests First → Showing → Around → Last position states
    /// </summary>
    [Fact]
    public void PlaybackControl_SubtitlePositionStateTransitions_WorkCorrectly()
    {
        // Arrange
        var config = new Config(true);
        var subManager = new SubManager(config, 0, false);
        var subtitles = new List<SubtitleData>
        {
            new() { StartTime = TimeSpan.FromSeconds(10), EndTime = TimeSpan.FromSeconds(15), Text = "Sub 1" },
            new() { StartTime = TimeSpan.FromSeconds(20), EndTime = TimeSpan.FromSeconds(25), Text = "Sub 2" },
            new() { StartTime = TimeSpan.FromSeconds(30), EndTime = TimeSpan.FromSeconds(35), Text = "Sub 3" }
        };
        subManager.Load(subtitles);

        // Act & Assert
        using (new AssertionScope())
        {
            // Before first subtitle
            subManager.SetCurrentTime(TimeSpan.FromSeconds(5));
            subManager.State.Should().Be(SubManager.PositionState.First);

            // During first subtitle
            subManager.SetCurrentTime(TimeSpan.FromSeconds(12));
            subManager.State.Should().Be(SubManager.PositionState.Showing);

            // Between subtitles
            subManager.SetCurrentTime(TimeSpan.FromSeconds(18));
            subManager.State.Should().Be(SubManager.PositionState.Around);

            // After last subtitle
            subManager.SetCurrentTime(TimeSpan.FromSeconds(40));
            subManager.State.Should().Be(SubManager.PositionState.Last);
        }
    }

    /// <summary>
    /// NEGATIVE: DeleteAfter removes all subtitles after given time
    /// Expected: Subtitles correctly removed
    /// </summary>
    [Fact]
    public void PlaybackControl_DeleteAfterTime_RemovesCorrectly()
    {
        // Arrange
        var config = new Config(true);
        var subManager = new SubManager(config, 0, false);
        var subtitles = new List<SubtitleData>
        {
            new() { StartTime = TimeSpan.FromSeconds(10), EndTime = TimeSpan.FromSeconds(15), Text = "Sub 1" },
            new() { StartTime = TimeSpan.FromSeconds(20), EndTime = TimeSpan.FromSeconds(25), Text = "Sub 2" },
            new() { StartTime = TimeSpan.FromSeconds(30), EndTime = TimeSpan.FromSeconds(35), Text = "Sub 3" }
        };
        subManager.Load(subtitles);

        // Act
        subManager.DeleteAfter(TimeSpan.FromSeconds(22));

        // Assert
        subManager.Subs.Count.Should().Be(1);
        subManager.Subs[0].Text.Should().Be("Sub 1");
    }
}
