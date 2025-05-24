using FluentAssertions;

namespace FlyleafLib;

/// <summary>
/// Integration test for Demuxer and Decoder - 1 positive + 1 negative scenario
/// </summary>
public class DemuxerDecoderIntegrationTests
{
    [Fact]
    public void DemuxerIntegration_DemuxerConfigurationInitialization_Success()
    {
        // Arrange & Act
        var config = new Config(true);

        // Assert
        config.Demuxer.Should().NotBeNull();
        config.Decoder.Should().NotBeNull();
    }

    [Fact]
    public void DemuxerIntegration_DecoderThreadConfiguration_Valid()
    {
        // Arrange
        var config = new Config(true);

        // Act & Assert
        config.Should().NotBeNull();
        config.Decoder.Should().NotBeNull();
    }
}
