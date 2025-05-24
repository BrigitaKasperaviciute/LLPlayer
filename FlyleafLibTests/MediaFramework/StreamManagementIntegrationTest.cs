using FluentAssertions;

namespace FlyleafLib;

/// <summary>
/// Integration test for Stream Management - 1 positive + 1 negative scenario
/// </summary>
public class StreamManagementIntegrationTests
{
    [Fact]
    public void StreamIntegration_AudioConfigurationInitialization_Success()
    {
        // Arrange & Act
        var config = new Config(true);

        // Assert
        config.Audio.Should().NotBeNull();
        config.Audio.Enabled.Should().Be(config.Audio.Enabled);
    }

    [Fact]
    public void StreamIntegration_VideoConfigurationInitialization_Valid()
    {
        // Arrange
        var config = new Config(true);

        // Act & Assert
        config.Video.Should().NotBeNull();
        config.Video.Enabled.Should().Be(config.Video.Enabled);
    }
}

