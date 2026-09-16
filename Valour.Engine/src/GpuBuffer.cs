using Silk.NET.OpenGL;

namespace Valour.Engine;

internal class GpuBuffer<T> : IDisposable where T : unmanaged
{
    private readonly GL _gl;
    private bool _disposed;

    public uint Handle { get; }
    public int Count { get; }

    public unsafe GpuBuffer(GL gl, ReadOnlySpan<T> data)
    {
        _gl = gl;
        
        Count = data.Length;
        
        _gl.CreateBuffers(1, out uint handle);
        Handle = handle;

        fixed (T* p = data)
            _gl.NamedBufferStorage(Handle, (nuint)(data.Length * sizeof(T)), p, BufferStorageMask.None);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _gl.DeleteBuffer(Handle);
            _disposed = true;
        }
    }
}