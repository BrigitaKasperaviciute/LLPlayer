using System.Windows;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace FlyleafLib.MediaPlayer;

/// <summary>
/// Integration tests for Configuration state management.
/// Tests verify config field consistency after mutations and detects inconsistencies.
/// </summary>
[Collection("WPF Application Collection")]
public class ConfigurationStateTests
{
    [Fact]
    public void ConfigurationState_Positive_ConfigFieldsStayConsistentAfterMutations()
    {
        // Arrange
        var config = new Config(true);
        var player = new Player(config);

        // Store initial configuration values
        bool initialAudioEnabled = config.Audio.Enabled;
        bool initialVideoEnabled = config.Video.Enabled;
        bool initialSubtitlesEnabled = config.Subtitles.Enabled;

        // Act - Perform multiple configuration mutations
        config.Audio.Enabled = false;
        config.Audio.Enabled = true;
        config.Video.Enabled = false;
        config.Video.Enabled = true;
        config.Subtitles.Enabled = false;
        config.Subtitles.Enabled = true;

        // Assert
        using (new AssertionScope())
        {
            // Verify config fields are consistent after mutations
            config.Audio.Enabled.Should().Be(initialAudioEnabled, "Audio.Enabled should return to initial value");
            config.Video.Enabled.Should().Be(initialVideoEnabled, "Video.Enabled should return to initial value");
            config.Subtitles.Enabled.Should().Be(initialSubtitlesEnabled, "Subtitles.Enabled should return to initial value");

            // Verify player reference to config is maintained
            player.Config.Should().BeSameAs(config, "Player should maintain reference to the same config instance");

            // Verify player is initialized
            player.PlayerId.Should().BeGreaterThan(0, "Player should have a valid ID");

            // Test that modifying volume preserves consistency
            int originalVolume = config.Player.VolumeMax;
            config.Player.VolumeMax = 150;
            config.Player.VolumeMax.Should().Be(150, "VolumeMax should be updated");
            config.Player.VolumeMax = originalVolume;
            config.Player.VolumeMax.Should().Be(originalVolume, "VolumeMax should return to original value");

            // Verify nested config objects are not null
            config.Audio.Should().NotBeNull("Audio config should remain initialized");
            config.Video.Should().NotBeNull("Video config should remain initialized");
            config.Subtitles.Should().NotBeNull("Subtitles config should remain initialized");
            config.Demuxer.Should().NotBeNull("Demuxer config should remain initialized");
            config.Decoder.Should().NotBeNull("Decoder config should remain initialized");
            config.Player.Should().NotBeNull("Player config should remain initialized");
        }

        // Cleanup
        player.Dispose();
    }

    [Fact]
    public void ConfigurationState_Negative_DetectsInconsistency()
    {
        // Arrange
        var config1 = new Config(true);
        var config2 = new Config(true);
        var player = new Player(config1);

        // Act & Assert - Test inconsistency scenarios
        using (new AssertionScope())
        {
            // Verify player is bound to config1, not config2
            player.Config.Should().BeSameAs(config1, "Player should be bound to config1");
            player.Config.Should().NotBeSameAs(config2, "Player should not be bound to config2");

            // Verify that creating a player with already-assigned config throws exception
            Action createWithUsedConfig = () => new Player(config1);
            createWithUsedConfig.Should().Throw<Exception>()
                .WithMessage("*already assigned*", "Config already assigned to a player should throw exception");

            // Test that Usage.Audio mode disables video and subtitles
            var audioOnlyConfig = new Config(true)
            {
                Player = { Usage = Usage.Audio }
            };
            var audioPlayer = new Player(audioOnlyConfig);

            audioOnlyConfig.Video.Enabled.Should().BeFalse("Video should be disabled in Audio-only mode");
            audioOnlyConfig.Subtitles.Enabled.Should().BeFalse("Subtitles should be disabled in Audio-only mode");
            audioOnlyConfig.Audio.Enabled.Should().BeTrue("Audio should remain enabled in Audio-only mode");

            // Verify this state is preserved and cannot be inconsistent
            audioOnlyConfig.Player.Usage.Should().Be(Usage.Audio, "Usage should remain Audio");

            // Test config independence - creating separate configs results in independent instances
            config2.Audio.Enabled = !config1.Audio.Enabled;
            config2.Audio.Enabled.Should().NotBe(config1.Audio.Enabled, "Separate config instances should be independent");

            // Cleanup audio player
            audioPlayer.Dispose();
        }

        // Cleanup
        player.Dispose();
    }
}
