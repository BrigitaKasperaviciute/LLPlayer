using System.Windows;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace FlyleafLib.MediaPlayer;

/// <summary>
/// Integration tests for Speed playback functionality.
/// Tests verify speed setting preservation and deviation detection.
/// </summary>
[Collection("WPF Application Collection")]
public class SpeedPlaybackTests
{
    [Fact]
    public void SpeedPlayback_Positive_SpeedSettingPreserved()
    {
        // Arrange
        var config = new Config(true);
        var player = new Player(config);
        double expectedSpeed = 1.5;

        // Act - Set playback speed
        player.Speed = expectedSpeed;

        // Assert
        using (new AssertionScope())
        {
            // Verify speed setting is preserved
            player.Speed.Should().Be(expectedSpeed, "Speed should be set to the requested value");

            // Verify speed is within valid range (0.125 - 16)
            player.Speed.Should().BeGreaterThanOrEqualTo(0.125, "Speed should not be below minimum");
            player.Speed.Should().BeLessThanOrEqualTo(16, "Speed should not exceed maximum");

            // Verify decoders receive speed setting
            player.AudioDecoder.Speed.Should().Be(expectedSpeed, "AudioDecoder should have the same speed");
            player.VideoDecoder.Speed.Should().Be(expectedSpeed, "VideoDecoder should have the same speed");

            // Test various valid speed values
            player.Speed = 0.5;
            player.Speed.Should().Be(0.5, "Speed should be updated to 0.5");

            player.Speed = 2.0;
            player.Speed.Should().Be(2.0, "Speed should be updated to 2.0");

            player.Speed = 1.0; // Reset to normal
            player.Speed.Should().Be(1.0, "Speed should reset to normal (1.0)");
        }

        // Cleanup
        player.Dispose();
    }

    [Fact]
    public void SpeedPlayback_Negative_DetectsDeviationFromSetSpeed()
    {
        // Arrange
        var config = new Config(true);
        var player = new Player(config);

        // Act & Assert - Test speed boundary violations
        using (new AssertionScope())
        {
            // Test below minimum speed (should clamp to 0.125)
            player.Speed = 0.05;
            player.Speed.Should().Be(0.125, "Speed below minimum should be clamped to 0.125");
            player.Speed.Should().NotBe(0.05, "Invalid speed value should be rejected");

            // Test above maximum speed (should clamp to 16)
            player.Speed = 20.0;
            player.Speed.Should().Be(16.0, "Speed above maximum should be clamped to 16.0");
            player.Speed.Should().NotBe(20.0, "Invalid speed value should be rejected");

            // Test negative speed (should clamp to minimum)
            player.Speed = -1.0;
            player.Speed.Should().Be(0.125, "Negative speed should be clamped to minimum");
            player.Speed.Should().NotBe(-1.0, "Negative speed should be rejected");

            // Verify speed consistency across decoders after invalid attempts
            player.Speed = 1.5;
            player.AudioDecoder.Speed.Should().Be(player.Speed, "AudioDecoder speed should match player speed");
            player.VideoDecoder.Speed.Should().Be(player.Speed, "VideoDecoder speed should match player speed");

            // Test that setting same speed twice doesn't cause issues
            double targetSpeed = 2.5;
            player.Speed = targetSpeed;
            double firstSet = player.Speed;
            player.Speed = targetSpeed;
            player.Speed.Should().Be(firstSet, "Setting same speed twice should maintain the value");

            // Test precision/rounding
            player.Speed = 1.23456789;
            player.Speed.Should().BeApproximately(1.235, 0.001, "Speed should be rounded to 3 decimal places");
        }

        // Cleanup
        player.Dispose();
    }
}
