using Silk.NET.OpenGL;

namespace Valour.Engine;

internal class VertexArray : IDisposable
{
    private bool _isDisposed;

    private readonly GL _gl;

    public uint Handle { get; private set; }
    public int IndexCount { get; private set; }
    public DrawElementsType IndexType { get; private set; } = DrawElementsType.UnsignedInt;

    public VertexArray(GL gl)
    {
        _gl = gl;

        _gl.CreateVertexArrays(1, out uint handle);
        Handle = handle;
    }

    public void Bind() => _gl.BindVertexArray(Handle);
    public void Unbind() => _gl.BindVertexArray(0);

    public void SetVertexBuffer(GpuBuffer<Vertex> vertexBuffer, uint bindingIndex, uint stride)
    {
        _gl.VertexArrayVertexBuffer(Handle, bindingIndex, vertexBuffer.Handle, 0, stride);
    }

    public void SetElementBuffer(GpuBuffer<uint> elementBuffer)
    {
        _gl.VertexArrayElementBuffer(Handle, elementBuffer.Handle);
        IndexCount = elementBuffer.Count;
    }

    public void EnableAttribute(uint attribIndex, uint bindingIndex, int size, VertexAttribType type, uint offset)
    {
        _gl.EnableVertexArrayAttrib(Handle, attribIndex);
        _gl.VertexArrayAttribFormat(Handle, attribIndex, size, type, false, offset);
        _gl.VertexArrayAttribBinding(Handle, attribIndex, bindingIndex);
    }

    public void Dispose()
    {
        if (!_isDisposed)
        {
            _gl.DeleteVertexArray(Handle);
            _isDisposed = true;
        }
    }
}