using Silk.NET.OpenGL;
using System.Numerics;
using System.Reflection;

namespace Valour.Engine;

public sealed class Shader : IDisposable
{
    private bool _disposed;

    private readonly GL _gl;
    private readonly uint _handle;

    internal Shader(GL gl, string vertSource, string fragSource)
    {
        _gl = gl;

        uint vert = 0, frag = 0;
        try
        {
            vert = CompileStage(ShaderType.VertexShader, vertSource);
            frag = CompileStage(ShaderType.FragmentShader, fragSource);

            _handle = _gl.CreateProgram();
            _gl.AttachShader(_handle, vert);
            _gl.AttachShader(_handle, frag);
            _gl.LinkProgram(_handle);

            _gl.GetProgram(_handle, ProgramPropertyARB.LinkStatus, out int ok);
            if (ok == 0)
            {
                string log = _gl.GetProgramInfoLog(_handle);
                _gl.DeleteProgram(_handle);
                throw new InvalidOperationException($"Shader program failed to link:\n{log}");
            }

            _gl.DetachShader(_handle, vert);
            _gl.DetachShader(_handle, frag);
        }
        finally
        {
            _gl.DeleteShader(vert);
            _gl.DeleteShader(frag);
        }
    }

    internal void Use()
    {
        _gl.UseProgram(_handle);
    }

    private uint CompileStage(ShaderType type, string source)
    {
        uint shader = _gl.CreateShader(type);
        _gl.ShaderSource(shader, source);
        _gl.CompileShader(shader);

        _gl.GetShader(shader, ShaderParameterName.CompileStatus, out int ok);
        if (ok == 0)
        {
            string log = _gl.GetShaderInfoLog(shader);
            _gl.DeleteShader(shader);
            throw new InvalidOperationException($"{type} failed to compile:\n{log}");
        }
        return shader;
    }

    internal void SetUniformMatrix4(string uniformName, Matrix4x4 matrix)
    {
        int location = _gl.GetUniformLocation(_handle, uniformName);

        unsafe
        {
            _gl.ProgramUniformMatrix4(_handle, location, 1, false, (float*)&matrix);
        }
    }

    internal void SetUniform4(string uniformName, Vector4 data)
    {
        int location = _gl.GetUniformLocation(_handle, uniformName);

        unsafe
        {
            _gl.ProgramUniform4(_handle, location, 1, (float*)&data);
        }
    }


    public void Dispose()
    {
        if (!_disposed)
        {
            _gl.DeleteProgram(_handle);
            _disposed = true;
        }
    }
}