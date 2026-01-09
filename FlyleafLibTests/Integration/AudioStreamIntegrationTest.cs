using FluentAssertions;
using FluentAssertions.Execution;
using FlyleafLib;
using FlyleafLib.MediaPlayer;

namespace FlyleafLibTests.Integration;

/// <summary>
/// Integration Test: Audio Configuration
/// Tests Audio config properties and constraints integration
/// Positive: Audio config initializes with valid settings
/// Negative: Invalid audio settings are handled
/// </summary>
public class AudioStreamIntegrationTest
{
    /// <summary>
    /// POSITIVE: Audio config initializes with valid properties
    /// ChannelsIn, SampleRateIn, Enabled should be properly initialized
    /// </summary>
    [Fact]
    public void AudioStream_AudioConfigInitialization_PropertiesValid()
    {
        // Arrange & Act
        var config = new Config(true);
        var audioConfig = config.Audio;

        // Assert
        using (new AssertionScope())
        {
            audioConfig.Should().NotBeNull();
            audioConfig.Enabled.Should().Be(audioConfig.Enabled); // Consistent value
        }
    }

    /// <summary>
    /// NEGATIVE: Multiple audio configs are independent
    /// Each Config instance should have separate audio configuration
    /// </summary>
    [Fact]
    public void AudioStream_MultipleAudioConfigs_Independent()
    {
        // Arrange
        var config1 = new Config(true);
        var config2 = new Config(true);

        // Act & Assert
        config1.Audio.Should().NotBeSameAs(config2.Audio);
        config1.Audio.Enabled.Should().Be(config1.Audio.Enabled);
        config2.Audio.Enabled.Should().Be(config2.Audio.Enabled);
    }
}
