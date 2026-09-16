using Silk.NET.Maths;
using Silk.NET.Windowing;

namespace Valour.Engine;

public class ValourEngine : IDisposable
{
    private readonly IWindow _window;
    private Renderer _renderer = null!;
    private IGame _game = null!;

    public ValourEngine(string title, int width, int height, bool fullscreen, bool vsync)
    {
        WindowOptions options = WindowOptions.Default;
        options.Title = title;
        options.Size = new Vector2D<int>(width, height);
        options.WindowState = fullscreen ? WindowState.Fullscreen : WindowState.Normal;
        options.VSync = vsync;
        options.API = new GraphicsAPI(ContextAPI.OpenGL, ContextProfile.Core, ContextFlags.ForwardCompatible, new APIVersion(4, 5));

        _window = Window.Create(options);

        _window.Load += OnLoad;
        _window.Update += OnUpdate;
        _window.Render += OnRender;
        _window.Closing += OnClose;
    }

    public void Run(IGame game)
    {
        _game = game;
        _window.Run();
    }

    private void OnLoad()
    {
        _renderer = new Renderer(_window);
        _game.Load(_renderer);
    }

    private void OnUpdate(double deltaTime)
    {
        _game.Update(deltaTime);
    }

    private void OnRender(double deltaTime)
    {
        _renderer.BeginFrame();
        _game.Render(_renderer);
        _renderer.EndFrame();
    }

    private void OnClose()
    {
        _game.Unload();
        _renderer?.Dispose();
    }

    public void Dispose()
    {
        _window.Dispose();
    }
}