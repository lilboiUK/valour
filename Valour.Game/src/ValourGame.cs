using Silk.NET.Input;
using System.Numerics;
using Valour.Engine;

namespace Valour.Game;

internal class ValourGame : IGame
{
    private Camera _camera;
    private float _cameraMoveSpeed = 2.0f;
    private float _cameraZoomSpeed = 5f;

    public Vector2 BoxOffset = new Vector2(0, 0);
    public Vector2 BoxSize = new Vector2(100, 100);
    public Vector4 BoxColor = new Vector4(1.0f, 1.0f, 0.0f, 1.0f);

    public void Load(Renderer renderer)
    {
        _camera = new Camera() { Zoom = 0.5f };
    }

    public void Update(double deltaTime, IInputContext input)
    {
        int inputX = 0;
        int inputY = 0;

        if (input.Keyboards[0].IsKeyPressed(Key.W)) inputY = 1;  // Up
        if (input.Keyboards[0].IsKeyPressed(Key.S)) inputY -= 1; // Down
        if (input.Keyboards[0].IsKeyPressed(Key.D)) inputX = 1;  // Right
        if (input.Keyboards[0].IsKeyPressed(Key.A)) inputX = -1;  // Left

        Vector2 moveVelocity = new Vector2(inputX, inputY) * _cameraMoveSpeed * (float)deltaTime;

        _camera.Position += moveVelocity;

        _camera.Zoom += input.Mice[0].ScrollWheels[0].Y * _cameraZoomSpeed * (float)deltaTime;
    }

    public void Render(Renderer renderer)
    {
        renderer.DrawSprite(new Vector2(0.0f, 0.0f), new Vector2(0.5f, 0.5f), _camera);
        renderer.DrawUiSprite(ScreenPoint.TopCenter, QuadPivot.TopCenter, new Vector2(0f, 30f), new Vector2(480f, 80f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f));
        renderer.DrawUiSprite(ScreenPoint.TopCenter, QuadPivot.TopCenter, BoxOffset, BoxSize, BoxColor);
    }

    public void Unload()
    {

    }
}