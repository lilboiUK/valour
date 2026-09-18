using Silk.NET.OpenGL;

namespace Valour.Engine;

public class Mesh : IDisposable
{
    private bool _disposed;

    internal VertexArray VertexArray { get; private set; }
    private readonly GpuBuffer<Vertex> _vbo;
    private readonly GpuBuffer<uint> _ebo;

    internal Mesh(GL gl, Vertex[] vertices, uint[] indices)
    {
        _vbo = new GpuBuffer<Vertex>(gl, vertices);
        _ebo = new GpuBuffer<uint>(gl, indices);
        VertexArray = new VertexArray(gl);

        VertexArray.SetVertexBuffer(_vbo, 0, Vertex.SizeInBytes);
        VertexArray.SetElementBuffer(_ebo);

        VertexArray.EnableAttribute(0, 0, 2, VertexAttribType.Float, Vertex.PositionOffset);
        VertexArray.EnableAttribute(1, 0, 2, VertexAttribType.Float, Vertex.TexCoordOffset);
    }

    internal void Bind() => VertexArray.Bind();

    public void Dispose()
    {
        if (!_disposed)
        {
            VertexArray.Dispose();
            _vbo.Dispose();
            _ebo.Dispose();
            _disposed = true;
        }
    }
}