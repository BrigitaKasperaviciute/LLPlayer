using FluentAssertions;
using FluentAssertions.Execution;
using FlyleafLib;
using FlyleafLib.MediaPlayer;

namespace FlyleafLibTests.Integration;

/// <summary>
/// Integration Test: Configuration System
/// Tests Config initialization and all subsystem configurations
/// Positive: Config initializes with all required subsystems
/// Negative: Config handles edge cases gracefully
/// </summary>
public class PlayerInitializationIntegrationTest
{
    /// <summary>
    /// POSITIVE: Config initializes successfully with all subsystems
    /// Verifies Demuxer, Decoder, Audio, Video, Player configs exist
    /// </summary>
    [Fact]
    public void PlayerInitialization_ConfigInitialization_AllSubsystemsReady()
    {
        // Arrange & Act
        var config = new Config(true);

        // Assert
        using (new AssertionScope())
        {
            config.Should().NotBeNull();
            
            // All subsystem configs should be initialized
            config.Demuxer.Should().NotBeNull();
            config.Decoder.Should().NotBeNull();
            config.Audio.Should().NotBeNull();
            config.Video.Should().NotBeNull();
            config.Player.Should().NotBeNull();
            
            // Config should have valid plugins collection
            config.Plugins.Should().NotBeNull();
        }
    }

    /// <summary>
    /// NEGATIVE: Multiple Config instances remain independent
    /// Changes to one Config should not affect another
    /// </summary>
    [Fact]
    public void PlayerInitialization_MultipleConfigInstances_AreIndependent()
    {
        // Arrange & Act
        var config1 = new Config(true);
        var config2 = new Config(true);

        // Assert
        config1.Should().NotBeSameAs(config2);
        config1.Player.Should().NotBeSameAs(config2.Player);
        config1.Audio.Should().NotBeSameAs(config2.Audio);
        config1.Video.Should().NotBeSameAs(config2.Video);
    }
}
