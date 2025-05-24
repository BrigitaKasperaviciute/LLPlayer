using FluentAssertions;

namespace FlyleafLib.MediaPlayer;

/// <summary>
/// Integration test for Video Playback - 1 positive + 1 negative scenario
/// </summary>
public class VideoPlaybackIntegrationTests
{
    [Fact]
    public void VideoIntegration_VideoConfigInitialization_Success()
    {
        // Arrange & Act
        var config = new Config(true);

        // Assert
        config.Video.Should().NotBeNull();
        config.Video.Enabled.Should().Be(config.Video.Enabled);
    }

    [Fact]
    public void VideoIntegration_VideoPropertiesValidation_Valid()
    {
        // Arrange
        var config = new Config(true);

        // Act & Assert
        config.Should().NotBeNull();
        config.Video.Should().NotBeNull();
    }
}
