using Silk.NET.Input;
using System.Numerics;
using Valour.Engine;

namespace Valour.Game;

internal class ValourGame : IGame
{
    private Shader _shader;
    private Mesh _mesh;
    private Camera _camera;
    private float _cameraMoveSpeed = 2.0f;
    private float _cameraZoomSpeed = 5f;

    public void Load(Renderer renderer)
    {
        string vertPath = Path.Combine(AppContext.BaseDirectory, "resources", "shader.vert");
        string fragPath = Path.Combine(AppContext.BaseDirectory, "resources", "shader.frag");

        string vertSource = File.ReadAllText(vertPath);
        string fragSource = File.ReadAllText(fragPath);

        _shader = renderer.CreateShader(vertSource, fragSource);

        Vertex[] vertices =
        [
            new(new(-0.5f, -0.5f), new(0.0f, 0.0f)), // bottom-left
            new(new( 0.5f, -0.5f), new(1.0f, 0.0f)), // bottom-right
            new(new(-0.5f,  0.5f), new(0.0f, 1.0f)), // top-left
            new(new( 0.5f,  0.5f), new(1.0f, 1.0f)), // top-right
        ];

        uint[] indices =
        [
            0, 1, 3,
            0, 3, 2
        ];

        _mesh = renderer.CreateMesh(vertices, indices);

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

        Console.WriteLine(input.Mice[0].Position);
    }

    public void Render(Renderer renderer)
    {
        renderer.Draw(_mesh, _shader, _camera);
    }

    public void Unload()
    {
        _shader.Dispose();
        _mesh.Dispose();
    }
}