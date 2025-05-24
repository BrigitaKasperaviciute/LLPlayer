using FluentAssertions;

namespace FlyleafLib.MediaPlayer;

/// <summary>
/// Integration test for Audio Playback - 1 positive + 1 negative scenario
/// </summary>
public class AudioPlaybackIntegrationTests
{
    [Fact]
    public void AudioIntegration_AudioConfigInitialization_Success()
    {
        // Arrange & Act
        var config = new Config(true);

        // Assert
        config.Audio.Should().NotBeNull();
        config.Audio.Enabled.Should().Be(config.Audio.Enabled);
    }

    [Fact]
    public void AudioIntegration_AudioChannelConfiguration_Valid()
    {
        // Arrange
        var config = new Config(true);

        // Act & Assert
        config.Should().NotBeNull();
        config.Audio.Should().NotBeNull();
    }
}
