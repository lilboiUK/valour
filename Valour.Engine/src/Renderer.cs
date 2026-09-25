using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using System.Numerics;

namespace Valour.Engine;

public class Renderer : IDisposable
{
    private readonly GL _gl;
    private readonly Shader _spriteShader;
    private readonly Mesh _spriteMesh;
    private readonly Vector2 _uiReferenceResolution = new Vector2(1920, 1080);

    private Vector2 _currentResolution;
    private float _aspectRatio;
    private float _uiScaleFactor;
    private Matrix4x4 _uiProjectionMatrix;

    internal Renderer(IView view)
    {
        _gl = GL.GetApi(view);

        Resize((Vector2)view.FramebufferSize);

        _spriteShader = CreateSpriteShader();
        _spriteMesh = CreateSpriteMesh();

        _gl.Enable(EnableCap.Blend);
        _gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
    }

    internal void BeginFrame()
    {
        _gl.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
        _gl.Clear(ClearBufferMask.ColorBufferBit);
    }

    internal void EndFrame() { }

    internal void Resize(Vector2 size)
    {
        _gl.Viewport(0, 0, (uint)size.X, (uint)size.Y);
        _currentResolution = size;
        _aspectRatio = size.Y == 0 ? 1.0f : size.X / size.Y;
        _uiProjectionMatrix = Matrix4x4.CreateOrthographicOffCenter(0.0f, size.X, size.Y, 0.0f, -1.0f, 1.0f);
        _uiScaleFactor = size.Y == 0 ? 1.0f : size.Y / _uiReferenceResolution.Y;
    }

    private Shader CreateSpriteShader()
    {
        string vertPath = Path.Combine(AppContext.BaseDirectory, "resources/shaders/sprite_shader.vert");
        string fragPath = Path.Combine(AppContext.BaseDirectory, "resources/shaders/sprite_shader.frag");
        string vertSource = File.ReadAllText(vertPath);
        string fragSource = File.ReadAllText(fragPath);
        return CreateShader(vertSource, fragSource);
    }

    private Mesh CreateSpriteMesh()
    {
        Vertex[] vertices =
        [
            new(new(-0.5f, -0.5f), new(0.0f, 0.0f)), // bottom-left
            new(new( 0.5f, -0.5f), new(1.0f, 0.0f)), // bottom-right
            new(new(-0.5f,  0.5f), new(0.0f, 1.0f)), // top-left
            new(new( 0.5f,  0.5f), new(1.0f, 1.0f)), // top-right
        ];

        uint[] indices = [0, 1, 3, 0, 3, 2];

        return CreateMesh(vertices, indices);
    }

    public void Dispose() => _gl.Dispose();

    public unsafe void DrawSprite(Vector2 position, Vector2 size, Camera camera)
    {
        _spriteShader.Use();
        _spriteMesh.Bind();

        Matrix4x4 model = Matrix4x4.CreateScale(new Vector3(size, 1.0f)) * Matrix4x4.CreateTranslation(new Vector3(position, 0.0f));
        Matrix4x4 view = camera.GetViewMatrix();
        Matrix4x4 projection = camera.GetProjectionMatrix(_aspectRatio);

        _spriteShader.SetUniformMatrix4("uMVP", model * view * projection);

        _gl.DrawElements(PrimitiveType.Triangles, _spriteMesh.VertexArray.IndexCount, _spriteMesh.VertexArray.IndexType, (void*)0);
    }

    public unsafe void DrawUiSprite(Vector2 anchor, Vector2 pivot, Vector2 offset, Vector2 size, Vector4 color)
    {
        _spriteShader.Use();
        _spriteMesh.Bind();

        Vector2 sizeScaled = size * _uiScaleFactor;

        Vector2 origin = (anchor * _currentResolution) + (offset * _uiScaleFactor) + (pivot * sizeScaled);

        Matrix4x4 model = Matrix4x4.CreateScale(sizeScaled.X, sizeScaled.Y, 1.0f) * Matrix4x4.CreateTranslation(origin.X, origin.Y, 0.0f);

        _spriteShader.SetUniformMatrix4("uMVP", model * _uiProjectionMatrix);
        _spriteShader.SetUniform4("uColor", color);

        _gl.DrawElements(PrimitiveType.Triangles, _spriteMesh.VertexArray.IndexCount, _spriteMesh.VertexArray.IndexType, (void*)0);
    }

    // Factory methods
    public Shader CreateShader(string vertSource, string fragSource) => new Shader(_gl, vertSource, fragSource);
    public Mesh CreateMesh(Vertex[] vertices, uint[] indices) => new Mesh(_gl, vertices, indices);
}