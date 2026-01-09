using FluentAssertions;
using FluentAssertions.Execution;
using FlyleafLib;
using FlyleafLib.MediaPlayer;

namespace FlyleafLibTests.Integration;

/// <summary>
/// Integration Test: Subtitles Configuration and State
/// Tests SubManager configuration with Config integration
/// Positive: SubManager works with Config properly
/// Negative: Invalid subtitle operations handled
/// </summary>
public class ConfigurationStateIntegrationTest
{
    /// <summary>
    /// POSITIVE: SubManager initializes and loads subtitles correctly
    /// Integration between SubManager and Config works
    /// </summary>
    [Fact]
    public void ConfigurationState_SubManagerWithConfig_WorksCorrectly()
    {
        // Arrange
        var config = new Config(true);
        var subManager = new SubManager(config, 0, false);
        var subtitles = new List<SubtitleData>
        {
            new() { StartTime = TimeSpan.Zero, EndTime = TimeSpan.FromSeconds(5), Text = "Test 1" },
            new() { StartTime = TimeSpan.FromSeconds(10), EndTime = TimeSpan.FromSeconds(15), Text = "Test 2" }
        };

        // Act
        subManager.Load(subtitles);

        // Assert
        using (new AssertionScope())
        {
            subManager.Subs.Count.Should().Be(2);
            subManager.Subs[0].Text.Should().Be("Test 1");
            subManager.Subs[1].Text.Should().Be("Test 2");
        }
    }

    /// <summary>
    /// NEGATIVE: Multiple SubManagers with different configs are independent
    /// </summary>
    [Fact]
    public void ConfigurationState_MultipleSubManagers_Independent()
    {
        // Arrange
        var config1 = new Config(true);
        var config2 = new Config(true);
        var subManager1 = new SubManager(config1, 0, false);
        var subManager2 = new SubManager(config2, 0, false);

        var subtitles1 = new List<SubtitleData>
        {
            new() { StartTime = TimeSpan.Zero, EndTime = TimeSpan.FromSeconds(5), Text = "Manager1" }
        };
        var subtitles2 = new List<SubtitleData>
        {
            new() { StartTime = TimeSpan.Zero, EndTime = TimeSpan.FromSeconds(5), Text = "Manager2" }
        };

        // Act
        subManager1.Load(subtitles1);
        subManager2.Load(subtitles2);

        // Assert
        subManager1.Subs[0].Text.Should().Be("Manager1");
        subManager2.Subs[0].Text.Should().Be("Manager2");
        subManager1.Should().NotBeSameAs(subManager2);
    }
}
