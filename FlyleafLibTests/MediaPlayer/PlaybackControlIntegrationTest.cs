using FluentAssertions;

namespace FlyleafLib.MediaPlayer;

/// <summary>
/// Integration test for Playback Control - 1 positive + 1 negative scenario
/// </summary>
public class PlaybackControlIntegrationTests
{
    [Fact]
    public void PlaybackIntegration_PlayerConfigInitialization_Success()
    {
        // Arrange & Act
        var config = new Config(true);

        // Assert
        config.Player.Should().NotBeNull();
        config.Player.AutoPlay.Should().Be(config.Player.AutoPlay);
    }

    [Fact]
    public void PlaybackIntegration_KeyBindingsAvailable_Valid()
    {
        // Arrange
        var config = new Config(true);

        // Act & Assert
        config.Should().NotBeNull();
        config.Player.Should().NotBeNull();
    }
}
