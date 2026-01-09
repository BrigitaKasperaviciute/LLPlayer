using System.Windows;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace FlyleafLib.MediaPlayer;

/// <summary>
/// Integration tests for Audio stream functionality.
/// Tests verify audio enabling/disabling behavior.
/// </summary>
[Collection("WPF Application Collection")]
public class AudioStreamTests
{
    [Fact]
    public void AudioStream_Positive_AudioEnabledByDefault()
    {
        // Arrange
        var config = new Config(true);
        var player = new Player(config);

        // Act & Assert
        using (new AssertionScope())
        {
            // Audio should be enabled by default in a standard configuration
            config.Audio.Enabled.Should().BeTrue("Audio should be enabled by default");

            // Verify Audio subsystem is properly initialized
            player.Audio.Should().NotBeNull("Audio subsystem should be created");
            player.AudioDecoder.Should().NotBeNull("AudioDecoder should be initialized");
            player.AudioDemuxer.Should().NotBeNull("AudioDemuxer should be initialized");

            // Verify audio is not opened yet (no media loaded)
            player.Audio.IsOpened.Should().BeFalse("Audio should not be opened without media");

            // Verify audio collections are available
            player.Audio.Streams.Should().NotBeNull("Audio streams collection should be initialized");

            // Verify initial audio state
            player.Audio.Volume.Should().BeGreaterThanOrEqualTo(0, "Volume should be initialized to valid value");
        }

        // Cleanup
        player.Dispose();
    }

    [Fact]
    public void AudioStream_Negative_AudioCanBeDisabled()
    {
        // Arrange
        var config = new Config(true)
        {
            Audio = { Enabled = false }
        };

        // Act
        var player = new Player(config);

        // Assert
        using (new AssertionScope())
        {
            // Verify audio is disabled in configuration
            config.Audio.Enabled.Should().BeFalse("Audio should be disabled when explicitly set to false");

            // Audio subsystem should still be created but disabled
            player.Audio.Should().NotBeNull("Audio subsystem should still be created");
            player.AudioDecoder.Should().NotBeNull("AudioDecoder should still be initialized");

            // Verify audio is not opened
            player.Audio.IsOpened.Should().BeFalse("Audio should not be opened when disabled");

            // Verify other subsystems are not affected by disabled audio
            player.Video.Should().NotBeNull("Video subsystem should be created");
            player.Subtitles.Should().NotBeNull("Subtitles subsystem should be created");

            // Verify player is still functional
            player.Status.Should().Be(Status.Stopped, "Player should be in Stopped status");
            player.IsDisposed.Should().BeFalse("Player should not be disposed");
        }

        // Cleanup
        player.Dispose();
    }
}
