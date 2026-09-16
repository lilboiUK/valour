using Silk.NET.OpenGL;

namespace Valour.Engine;

public class Mesh : IDisposable
{
    private bool _disposed;

    private readonly VertexArray _vao;
    private readonly GpuBuffer<Vertex> _vbo;
    private readonly GpuBuffer<uint> _ebo;

    public int IndicesCount { get; }

    internal Mesh(GL gl, Vertex[] vertices, uint[] indices)
    {
        IndicesCount = indices.Length;

        _vbo = new GpuBuffer<Vertex>(gl, vertices);
        _ebo = new GpuBuffer<uint>(gl, indices);
        _vao = new VertexArray(gl);

        _vao.SetVertexBuffer(_vbo, 0, Vertex.SizeInBytes);
        _vao.SetElementBuffer(_ebo);

        _vao.EnableAttribute(0, 0, 2, VertexAttribType.Float, Vertex.PositionOffset);
        _vao.EnableAttribute(1, 0, 2, VertexAttribType.Float, Vertex.TexCoordOffset);
    }

    internal void Use() => _vao.Bind();

    public void Dispose()
    {
        if (!_disposed)
        {
            _vao.Dispose();
            _vbo.Dispose();
            _ebo.Dispose();
            _disposed = true;
        }
    }
}