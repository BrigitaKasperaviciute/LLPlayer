using System.Windows;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace FlyleafLib.MediaPlayer;

/// <summary>
/// Integration tests for Video stream functionality.
/// Tests verify video enabling/disabling behavior.
/// </summary>
[Collection("WPF Application Collection")]
public class VideoStreamTests
{
    [Fact]
    public void VideoStream_Positive_VideoEnabledByDefault()
    {
        // Arrange
        var config = new Config(true);
        var player = new Player(config);

        // Act & Assert
        using (new AssertionScope())
        {
            // Video should be enabled by default in a standard configuration
            config.Video.Enabled.Should().BeTrue("Video should be enabled by default");

            // Verify Video subsystem is properly initialized
            player.Video.Should().NotBeNull("Video subsystem should be created");
            player.VideoDecoder.Should().NotBeNull("VideoDecoder should be initialized");
            player.VideoDemuxer.Should().NotBeNull("VideoDemuxer should be initialized");

            // Verify video is not opened yet (no media loaded)
            player.Video.IsOpened.Should().BeFalse("Video should not be opened without media");

            // Verify video collections are available
            player.Video.Streams.Should().NotBeNull("Video streams collection should be initialized");

            // Verify renderer is initialized
            player.renderer.Should().NotBeNull("Renderer should be initialized");

            // Verify initial video state properties exist
            player.Video.Width.Should().Be(0, "Width should be 0 without media");
            player.Video.Height.Should().Be(0, "Height should be 0 without media");
            player.Video.FPS.Should().Be(0, "FPS should be 0 without media");
        }

        // Cleanup
        player.Dispose();
    }

    [Fact]
    public void VideoStream_Negative_VideoCanBeDisabled()
    {
        // Arrange
        var config = new Config(true)
        {
            Video = { Enabled = false }
        };

        // Act
        var player = new Player(config);

        // Assert
        using (new AssertionScope())
        {
            // Verify video is disabled in configuration
            config.Video.Enabled.Should().BeFalse("Video should be disabled when explicitly set to false");

            // Video subsystem should still be created but disabled
            player.Video.Should().NotBeNull("Video subsystem should still be created");
            player.VideoDecoder.Should().NotBeNull("VideoDecoder should still be initialized");

            // Verify video is not opened
            player.Video.IsOpened.Should().BeFalse("Video should not be opened when disabled");

            // Verify other subsystems are not affected by disabled video
            player.Audio.Should().NotBeNull("Audio subsystem should be created");
            player.Subtitles.Should().NotBeNull("Subtitles subsystem should be created");

            // Verify player is still functional
            player.Status.Should().Be(Status.Stopped, "Player should be in Stopped status");
            player.IsDisposed.Should().BeFalse("Player should not be disposed");

            // Video properties should be in initial state
            player.Video.Width.Should().Be(0, "Width should be 0");
            player.Video.Height.Should().Be(0, "Height should be 0");
        }

        // Cleanup
        player.Dispose();
    }
}
