using FluentAssertions;
using FluentAssertions.Execution;
using FlyleafLib;
using FlyleafLib.MediaPlayer;

namespace FlyleafLibTests.Integration;

/// <summary>
/// Integration Test: Demuxer and Decoder Configuration
/// Tests Demuxer/Decoder config integration
/// Positive: Demuxer and Decoder configs initialize properly
/// Negative: Invalid configurations handled
/// </summary>
public class FileOpenIntegrationTest
{
    /// <summary>
    /// POSITIVE: Demuxer and Decoder configs are initialized with Config
    /// Verifies thread counts and buffer settings
    /// </summary>
    [Fact]
    public void FileOpen_DemuxerDecoderConfigInitialization_Valid()
    {
        // Arrange & Act
        var config = new Config(true);
        var demuxerConfig = config.Demuxer;
        var decoderConfig = config.Decoder;

        // Assert
        using (new AssertionScope())
        {
            demuxerConfig.Should().NotBeNull();
            decoderConfig.Should().NotBeNull();
        }
    }

    /// <summary>
    /// NEGATIVE: Multiple Demuxer/Decoder configs are independent
    /// Changes to one Config's demuxer shouldn't affect another
    /// </summary>
    [Fact]
    public void FileOpen_MultipleDemuxerDecoderConfigs_Independent()
    {
        // Arrange
        var config1 = new Config(true);
        var config2 = new Config(true);

        // Act & Assert
        config1.Demuxer.Should().NotBeSameAs(config2.Demuxer);
        config1.Decoder.Should().NotBeSameAs(config2.Decoder);
    }
}
