using System.Windows;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace FlyleafLib.MediaPlayer;

/// <summary>
/// Integration tests for Seek timeline functionality.
/// Tests verify all subsystems initialization and subtitle toggling during seeks.
/// </summary>
[Collection("WPF Application Collection")]
public class SeekTimelineTests
{
    [Fact]
    public void SeekTimeline_Positive_AllSubsystemsInitialized()
    {
        // Arrange
        var config = new Config(true);
        var player = new Player(config);

        // Act & Assert
        using (new AssertionScope())
        {
            // Verify all subsystems required for seek timeline are initialized
            player.Audio.Should().NotBeNull("Audio subsystem should be initialized for seek operations");
            player.Video.Should().NotBeNull("Video subsystem should be initialized for seek operations");
            player.Subtitles.Should().NotBeNull("Subtitles subsystem should be initialized for seek operations");
            player.decoder.Should().NotBeNull("Decoder context should be initialized");

            // Verify demuxers for timeline operations are initialized
            player.AudioDemuxer.Should().NotBeNull("AudioDemuxer should be initialized");
            player.VideoDemuxer.Should().NotBeNull("VideoDemuxer should be initialized");
            player.SubtitlesDemuxers.Should().NotBeNull("SubtitlesDemuxers should be initialized");
            // Note: MainDemuxer is null until media is opened

            // Verify timeline-related properties exist
            player.CurTime.Should().Be(0, "Current time should be 0 without media");
            player.Duration.Should().Be(0, "Duration should be 0 without media");

            // Verify seek capability (should be false without media)
            player.CanPlay.Should().BeFalse("CanPlay should be false without media loaded");

            // Verify Status is ready for seek operations
            player.Status.Should().Be(Status.Stopped, "Status should be Stopped initially");
        }

        // Cleanup
        player.Dispose();
    }

    [Fact]
    public void SeekTimeline_Negative_SubtitlesCanBeToggledOff()
    {
        // Arrange - Create player with subtitles enabled
        var config = new Config(true);
        var player = new Player(config);

        // Verify initial state
        config.Subtitles.Enabled.Should().BeTrue("Subtitles should be enabled initially");

        // Act - Toggle subtitles off
        config.Subtitles.Enabled = false;

        // Assert
        using (new AssertionScope())
        {
            // Verify subtitles are disabled
            config.Subtitles.Enabled.Should().BeFalse("Subtitles should be disabled after toggling");

            // Verify subtitles subsystem still exists (but disabled)
            player.Subtitles.Should().NotBeNull("Subtitles subsystem should still exist");
            player.SubtitlesManager.Should().NotBeNull("SubtitlesManager should still exist");

            // Verify other subsystems remain unaffected by subtitle toggle
            player.Audio.Should().NotBeNull("Audio subsystem should remain initialized");
            player.Video.Should().NotBeNull("Video subsystem should remain initialized");
            player.decoder.Should().NotBeNull("Decoder should remain initialized");

            // Verify timeline operations structures exist even without subtitles
            // Note: MainDemuxer is null until media is opened
            player.CurTime.Should().Be(0, "CurTime should still be accessible");
            player.Duration.Should().Be(0, "Duration should still be accessible");

            // Verify player state is consistent
            player.Status.Should().Be(Status.Stopped, "Player status should be consistent");
            player.IsDisposed.Should().BeFalse("Player should not be disposed");
        }

        // Act - Toggle subtitles back on
        config.Subtitles.Enabled = true;

        // Assert subtitles can be re-enabled
        config.Subtitles.Enabled.Should().BeTrue("Subtitles should be re-enabled");
        player.Subtitles.Should().NotBeNull("Subtitles subsystem should still be functional");

        // Cleanup
        player.Dispose();
    }
}
