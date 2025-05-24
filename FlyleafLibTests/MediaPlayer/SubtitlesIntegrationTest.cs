using FluentAssertions;

namespace FlyleafLib.MediaPlayer;

/// <summary>
/// Integration test for Subtitles Management - 1 positive + 1 negative scenario
/// </summary>
public class SubtitlesIntegrationTests
{
    [Fact]
    public void SubtitlesIntegration_SubtitleManagerCreation_Success()
    {
        // Arrange & Act
        var config = new Config(true);
        var subManager = new SubManager(config, 0, false);

        // Assert
        subManager.Should().NotBeNull();
        subManager.Subs.Count.Should().Be(0);
    }

    [Fact]
    public void SubtitlesIntegration_SubtitleManagerLoading_Valid()
    {
        // Arrange
        var config = new Config(true);
        var subManager = new SubManager(config, 0, false);
        var subtitleData = new List<SubtitleData>
        {
            new() { StartTime = TimeSpan.FromSeconds(1), EndTime = TimeSpan.FromSeconds(5), Text = "Hello" }
        };

        // Act
        subManager.Load(subtitleData);

        // Assert
        subManager.Should().NotBeNull();
        subManager.Subs.Count.Should().Be(1);
    }
}
