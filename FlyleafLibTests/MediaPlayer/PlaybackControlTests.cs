using System.Windows;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace FlyleafLib.MediaPlayer;

/// <summary>
/// Integration tests for Playback control functionality.
/// Tests verify subtitle enabling/disabling behavior during playback control.
/// </summary>
[Collection("WPF Application Collection")]
public class PlaybackControlTests
{
    [Fact]
    public void PlaybackControl_Positive_SubtitlesEnabledByDefault()
    {
        // Arrange
        var config = new Config(true);
        var player = new Player(config);

        // Act & Assert
        using (new AssertionScope())
        {
            // Subtitles should be enabled by default in a standard configuration
            config.Subtitles.Enabled.Should().BeTrue("Subtitles should be enabled by default");

            // Verify subtitles subsystem is available
            player.Subtitles.Should().NotBeNull("Subtitles subsystem should be created");
            player.SubtitlesDecoders.Should().NotBeNull("Subtitles decoders should be initialized");
            player.SubtitlesManager.Should().NotBeNull("SubtitlesManager should be initialized");

            // Verify player is ready for playback operations
            player.Commands.Should().NotBeNull("Commands should be available for playback control");
            player.Status.Should().Be(Status.Stopped, "Initial status should be Stopped");
        }

        // Cleanup
        player.Dispose();
    }

    [Fact]
    public void PlaybackControl_Negative_SubtitlesCanBeDisabled()
    {
        // Arrange
        var config = new Config(true)
        {
            Subtitles = { Enabled = false }
        };

        // Act
        var player = new Player(config);

        // Assert
        using (new AssertionScope())
        {
            // Verify subtitles are disabled in configuration
            config.Subtitles.Enabled.Should().BeFalse("Subtitles should be disabled when explicitly set to false");

            // Subtitles subsystem should still be created but not enabled
            player.Subtitles.Should().NotBeNull("Subtitles subsystem should still be created");

            // Verify playback control is still functional without subtitles
            player.Commands.Should().NotBeNull("Playback commands should be available");
            player.Status.Should().Be(Status.Stopped, "Player should be in Stopped status");
            player.CanPlay.Should().BeFalse("Player should not be ready to play without media");

            // Verify other subsystems are not affected
            player.Audio.Should().NotBeNull("Audio subsystem should be created");
            player.Video.Should().NotBeNull("Video subsystem should be created");
        }

        // Cleanup
        player.Dispose();
    }
}
