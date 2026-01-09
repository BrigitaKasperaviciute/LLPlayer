using System.Windows;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace FlyleafLib.MediaPlayer;

/// <summary>
/// Integration tests for Player initialization subsystems.
/// Tests verify that Player, Audio, Video, Subtitles, and Demuxer subsystems are properly created.
/// </summary>
[Collection("WPF Application Collection")]
public class PlayerInitializationTests
{
    [Fact]
    public void PlayerInitialization_Positive_AllSubsystemsCreated()
    {
        // Arrange
        var config = new Config(true);
        var player = new Player(config);

        // Assert
        using (new AssertionScope())
        {
            player.Should().NotBeNull("player instance should be created");
            player.PlayerId.Should().BeGreaterThan(0, "player should have a unique ID");

            // Verify all subsystems are initialized
            player.Audio.Should().NotBeNull("Audio subsystem should be created");
            player.Video.Should().NotBeNull("Video subsystem should be created");
            player.Subtitles.Should().NotBeNull("Subtitles subsystem should be created");
            player.decoder.Should().NotBeNull("Demuxer should be created");
            player.Data.Should().NotBeNull("Data subsystem should be created");
            player.Commands.Should().NotBeNull("Commands subsystem should be created");
            player.Activity.Should().NotBeNull("Activity subsystem should be created");

            // Verify decoder context subsystems
            player.AudioDecoder.Should().NotBeNull("AudioDecoder should be initialized");
            player.VideoDecoder.Should().NotBeNull("VideoDecoder should be initialized");
            player.SubtitlesDecoders.Should().NotBeNull("SubtitlesDecoders should be initialized");
            player.DataDecoder.Should().NotBeNull("DataDecoder should be initialized");

            // Verify initial state
            player.Status.Should().Be(Status.Stopped, "initial status should be Stopped");
            player.IsPlaying.Should().BeFalse("player should not be playing initially");
            player.IsDisposed.Should().BeFalse("player should not be disposed");
        }

        // Cleanup
        player.Dispose();
    }

    [Fact]
    public void PlayerInitialization_Negative_CustomAudioFlagStaysOff()
    {
        // Arrange
        var config = new Config(true)
        {
            Player = { Usage = Usage.Audio }
        };

        // Act
        var player = new Player(config);

        // Assert
        using (new AssertionScope())
        {
            // When Usage is set to Audio, video and subtitles should be disabled
            config.Video.Enabled.Should().BeFalse("Video should be disabled when Usage is Audio");
            config.Subtitles.Enabled.Should().BeFalse("Subtitles should be disabled when Usage is Audio");

            // Audio subsystem should still be created even if disabled
            player.Audio.Should().NotBeNull("Audio subsystem should always be created");
            player.Video.Should().NotBeNull("Video subsystem should be created (even if disabled)");
            player.Subtitles.Should().NotBeNull("Subtitles subsystem should be created (even if disabled)");

            // Verify the usage mode is preserved
            config.Player.Usage.Should().Be(Usage.Audio, "Usage mode should remain as Audio");
        }

        // Cleanup
        player.Dispose();
    }
}
