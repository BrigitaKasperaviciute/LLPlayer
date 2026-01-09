using FluentAssertions;
using FluentAssertions.Execution;
using FlyleafLib;
using FlyleafLib.MediaPlayer;

namespace FlyleafLibTests.Integration;

/// <summary>
/// Integration Test: Player Configuration Speed Settings
/// Tests Player config speed options
/// Positive: Speed settings initialize with valid defaults
/// Negative: Multiple configs have independent settings
/// </summary>
public class SpeedPlaybackIntegrationTest
{
    /// <summary>
    /// POSITIVE: Player config speed options are properly initialized
    /// </summary>
    [Fact]
    public void SpeedPlayback_PlayerSpeedConfig_Initialized()
    {
        // Arrange & Act
        var config = new Config(true);
        var playerConfig = config.Player;

        // Assert
        using (new AssertionScope())
        {
            playerConfig.Should().NotBeNull();
            playerConfig.SpeedOffset.Should().BeGreaterThanOrEqualTo(0);
        }
    }

    /// <summary>
    /// NEGATIVE: Multiple player configs have independent speed settings
    /// </summary>
    [Fact]
    public void SpeedPlayback_MultiplePlayerConfigs_IndependentSpeed()
    {
        // Arrange
        var config1 = new Config(true);
        var config2 = new Config(true);

        // Act & Assert
        config1.Player.Should().NotBeSameAs(config2.Player);
    }
}
