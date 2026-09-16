using Silk.NET.Maths;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Valour.Engine;

[StructLayout(LayoutKind.Sequential)]
public struct Vertex(Vector2D<float> position, Vector2D<float> texCoords)
{
    public Vector2D<float> Position = position;
    public Vector2D<float> TexCoords = texCoords;

    public static readonly uint SizeInBytes = (uint)Unsafe.SizeOf<Vertex>();
    public static readonly uint PositionOffset = (uint)Marshal.OffsetOf<Vertex>(nameof(Position));
    public static readonly uint TexCoordOffset = (uint)Marshal.OffsetOf<Vertex>(nameof(TexCoords));
}