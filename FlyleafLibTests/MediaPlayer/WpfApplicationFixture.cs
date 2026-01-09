using System.Windows;
using FlyleafLib;
using Xunit;

namespace FlyleafLib.MediaPlayer;

[CollectionDefinition("WPF Application Collection")]
public class WpfApplicationCollection : ICollectionFixture<WpfApplicationFixture>
{
}

public class WpfApplicationFixture : IDisposable
{
    public WpfApplicationFixture()
    {
        if (Application.Current == null)
            new Application();

        if (!Engine.IsLoaded)
            Engine.Start(new EngineConfig { SkipFFmpegLoad = true });
    }

    public void Dispose()
    {
        // No-op for now; letting test host manage shutdown
    }
}
