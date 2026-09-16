using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace Valour.Engine;

public class Renderer : IDisposable
{
    private readonly IView _view;
    private readonly GL _gl;

    internal Renderer(IView view)
    {
        _view = view;
        _gl = GL.GetApi(view);

        OnFramebufferResize(view.FramebufferSize);
        _view.FramebufferResize += OnFramebufferResize;
    }

    internal void BeginFrame()
    {
        _gl.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
        _gl.Clear(ClearBufferMask.ColorBufferBit);
    }

    internal void EndFrame()
    {

    }

    private void OnFramebufferResize(Vector2D<int> size)
    {
        _gl.Viewport(size);
    }

    public void Dispose()
    {
        _view.FramebufferResize -= OnFramebufferResize;
        _gl.Dispose();
    }

    public unsafe void Draw(Mesh mesh, Shader shader)
    {
        shader.Use();
        mesh.Use();
        _gl.DrawElements(PrimitiveType.Triangles, (uint)mesh.IndicesCount, DrawElementsType.UnsignedInt, (void*)0);
    }

    // Factory methods
    public Shader CreateShader(string vertSource, string fragSource) => new Shader(_gl, vertSource, fragSource);
    public Mesh CreateMesh(Vertex[] vertices, uint[] indices) => new Mesh(_gl, vertices, indices);
}