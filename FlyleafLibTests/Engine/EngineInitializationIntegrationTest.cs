using FluentAssertions;

namespace FlyleafLib;

/// <summary>
/// Integration test for Engine Initialization - 1 positive + 1 negative scenario
/// </summary>
public class EngineInitializationIntegrationTests
{
    [Fact]
    public void EngineIntegration_ConfigInitialization_Success()
    {
        // Arrange & Act
        var config = new Config(true);

        // Assert
        config.Should().NotBeNull();
        config.Audio.Should().NotBeNull();
        config.Video.Should().NotBeNull();
        config.Player.Should().NotBeNull();
    }

    [Fact]
    public void EngineIntegration_MultipleConfigInstances_Independent()
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
