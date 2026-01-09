using FluentAssertions;
using FluentAssertions.Execution;
using FlyleafLib;
using FlyleafLib.MediaPlayer;

namespace FlyleafLibTests.Integration;

/// <summary>
/// Integration Test: Subtitle Manager and Seek Integration
/// Tests SubManager.Load(), SetCurrentTime(), GetCurrent() working together
/// Positive: Loading subtitles and seeking through them works correctly
/// Negative: Loading invalid/null subtitle data throws or handled gracefully
/// </summary>
public class SubtitleSeekIntegrationTest
{
    /// <summary>
    /// POSITIVE: SubManager loads subtitles and seek correctly returns current subtitle
    /// Tests full subtitle lifecycle: Load → Seek → Get Current
    /// </summary>
    [Fact]
    public void SubtitleSeek_LoadSubtitlesAndSeek_ReturnsCorrectSubtitle()
    {
        // Arrange
        var config = new Config(true);
        var subManager = new SubManager(config, 0, false);
        
        var subtitles = new List<SubtitleData>
        {
            new() { StartTime = TimeSpan.FromSeconds(1), EndTime = TimeSpan.FromSeconds(5), Text = "1. First subtitle" },
            new() { StartTime = TimeSpan.FromSeconds(10), EndTime = TimeSpan.FromSeconds(15), Text = "2. Second subtitle" },
            new() { StartTime = TimeSpan.FromSeconds(20), EndTime = TimeSpan.FromSeconds(25), Text = "3. Third subtitle" }
        };

        // Act
        subManager.Load(subtitles);

        // Assert - Verify load worked
        using (new FluentAssertions.Execution.AssertionScope())
        {
            subManager.Subs.Count.Should().Be(3);
            
            // Seek to first subtitle
            subManager.SetCurrentTime(TimeSpan.FromSeconds(2));
            var current1 = subManager.GetCurrent();
            current1.Should().NotBeNull();
            current1!.Text.Should().Be("1. First subtitle");
            
            // Seek to second subtitle
            subManager.SetCurrentTime(TimeSpan.FromSeconds(12));
            var current2 = subManager.GetCurrent();
            current2.Should().NotBeNull();
            current2!.Text.Should().Be("2. Second subtitle");
            
            // Seek to third subtitle
            subManager.SetCurrentTime(TimeSpan.FromSeconds(22));
            var current3 = subManager.GetCurrent();
            current3.Should().NotBeNull();
            current3!.Text.Should().Be("3. Third subtitle");
        }
    }

    /// <summary>
    /// NEGATIVE: Loading null or empty subtitle list is handled gracefully
    /// Expected: Empty list or exception handled
    /// </summary>
    [Fact]
    public void SubtitleSeek_LoadEmptySubtitles_HandledGracefully()
    {
        // Arrange
        var config = new Config(true);
        var subManager = new SubManager(config, 0, false);

        // Act
        subManager.Load(new List<SubtitleData>()); // Empty list
        subManager.SetCurrentTime(TimeSpan.FromSeconds(5));

        // Assert
        subManager.Subs.Count.Should().Be(0);
        subManager.GetCurrent().Should().BeNull();
        subManager.GetNext().Should().BeNull();
        subManager.GetPrev().Should().BeNull();
    }
}
