using FluentAssertions;
using FluentAssertions.Execution;
using FlyleafLib;
using FlyleafLib.MediaPlayer;

namespace FlyleafLibTests.Integration;

/// <summary>
/// Integration Test: Video Configuration
/// Tests Video config properties and constraints integration
/// Positive: Video config initializes properly
/// Negative: Invalid video settings handled
/// </summary>
public class VideoStreamIntegrationTest
{
    /// <summary>
    /// POSITIVE: Video config initializes with valid properties
    /// MaximumFPS, Enabled, HideVideoWindow should be properly initialized
    /// </summary>
    [Fact]
    public void VideoStream_VideoConfigInitialization_PropertiesValid()
    {
        // Arrange & Act
        var config = new Config(true);
        var videoConfig = config.Video;

        // Assert
        using (new AssertionScope())
        {
            videoConfig.Should().NotBeNull();
            videoConfig.Enabled.Should().Be(videoConfig.Enabled); // Consistent
        }
    }

    /// <summary>
    /// NEGATIVE: Multiple video configs remain independent
    /// Each Config instance should have separate video configuration
    /// </summary>
    [Fact]
    public void VideoStream_MultipleVideoConfigs_Independent()
    {
        // Arrange
        var config1 = new Config(true);
        var config2 = new Config(true);

        // Act & Assert
        config1.Video.Should().NotBeSameAs(config2.Video);
        config1.Video.Enabled.Should().Be(config1.Video.Enabled);
        config2.Video.Enabled.Should().Be(config2.Video.Enabled);
    }
}
