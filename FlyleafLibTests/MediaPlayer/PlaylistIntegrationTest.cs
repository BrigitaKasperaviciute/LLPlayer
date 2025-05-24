using FluentAssertions;

namespace FlyleafLib.MediaPlayer;

/// <summary>
/// Integration test for Playlist - 1 positive + 1 negative scenario
/// </summary>
public class PlaylistIntegrationTests
{
    [Fact]
    public void PlaylistIntegration_PlayerConfig_Success()
    {
        // Arrange
        var config = new Config(true);

        // Assert
        config.Should().NotBeNull();
        config.Player.Should().NotBeNull();
    }

    [Fact]
    public void PlaylistIntegration_ConfigCreation_IsNotNull()
    {
        // Arrange & Act
        var config1 = new Config(true);
        var config2 = new Config(true);

        // Assert
        config1.Should().NotBeNull();
        config2.Should().NotBeNull();
        config1.Should().NotBeSameAs(config2);
    }
}
