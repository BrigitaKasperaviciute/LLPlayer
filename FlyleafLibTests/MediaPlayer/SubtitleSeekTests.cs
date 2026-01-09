using System.Windows;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace FlyleafLib.MediaPlayer;

/// <summary>
/// Integration tests for Subtitle seeking functionality.
/// Tests verify subtitle initialization and track count constraints.
/// </summary>
[Collection("WPF Application Collection")]
public class SubtitleSeekTests
{
    [Fact]
    public void SubtitleSeek_Positive_SubtitlesInitialized()
    {
        // Arrange
        var config = new Config(true);
        var player = new Player(config);

        // Act & Assert
        using (new AssertionScope())
        {
            // Verify Subtitles subsystem is properly initialized
            player.Subtitles.Should().NotBeNull("Subtitles subsystem should be created");
            player.SubtitlesDecoders.Should().NotBeNull("SubtitlesDecoders should be initialized");
            player.SubtitlesManager.Should().NotBeNull("SubtitlesManager should be initialized");
            player.SubtitlesDemuxers.Should().NotBeNull("SubtitlesDemuxers should be initialized");

            // Verify subtitles configuration
            config.Subtitles.Enabled.Should().BeTrue("Subtitles should be enabled by default");

            // Verify subtitle streams collection exists
            player.Subtitles.Streams.Should().NotBeNull("Subtitles streams collection should be initialized");

            // Verify subtitles are not opened yet (no media loaded)
            player.Subtitles[0].IsOpened.Should().BeFalse("Subtitles should not be opened without media");

            // Verify subtitle-related components
            player.SubtitlesOCR.Should().NotBeNull("SubtitlesOCR should be initialized");
            player.SubtitlesASR.Should().NotBeNull("SubtitlesASR should be initialized");
        }

        // Cleanup
        player.Dispose();
    }

    [Fact]
    public void SubtitleSeek_Negative_TrackCountFixed()
    {
        // Arrange
        var config = new Config(true);
        var player = new Player(config);

        // Act & Assert
        using (new AssertionScope())
        {
            // Verify the subtitle decoders array has a fixed size of 2
            // This is based on the Player.cs implementation where subNum is typically 2
            player.SubtitlesDecoders.Should().NotBeNull("SubtitlesDecoders should be initialized");
            player.SubtitlesDecoders.Length.Should().Be(2, "SubtitlesDecoders should have exactly 2 slots");

            // Verify subtitle demuxers also has fixed size
            player.SubtitlesDemuxers.Should().NotBeNull("SubtitlesDemuxers should be initialized");
            player.SubtitlesDemuxers.Length.Should().Be(2, "SubtitlesDemuxers should have exactly 2 slots");

            // Verify this constraint persists across configuration changes
            config.Subtitles.Enabled = false;
            config.Subtitles.Enabled = true;

            // Track count should remain fixed regardless of configuration changes
            player.SubtitlesDecoders.Length.Should().Be(2, "SubtitlesDecoders count should remain fixed at 2");
            player.SubtitlesDemuxers.Length.Should().Be(2, "SubtitlesDemuxers count should remain fixed at 2");

            // Verify subtitles subsystem remains functional
            player.Subtitles.Should().NotBeNull("Subtitles subsystem should remain initialized");
            player.SubtitlesManager.Should().NotBeNull("SubtitlesManager should remain initialized");
        }

        // Cleanup
        player.Dispose();
    }
}
