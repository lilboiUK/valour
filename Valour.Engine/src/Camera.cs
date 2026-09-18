using System.Numerics;

namespace Valour.Engine;

public class Camera
{
    private float _zoom = 1.0f;

    public float MinZoom { get; set; } = 0.1f;
    public float MaxZoom { get; set; } = 1.0f;

    public Vector2 Position { get; set; } = Vector2.Zero;

    public float Zoom
    {
        get => _zoom;
        set => _zoom = Math.Clamp(value, MinZoom, MaxZoom);
    }

    public Camera() { }

    public Matrix4x4 GetViewMatrix()
    {
        return Matrix4x4.CreateTranslation(new Vector3(-Position, 0.0f));
    }

    public Matrix4x4 GetProjectionMatrix(float aspectRatio)
    {
        float height = (1.0f / Zoom) * 2.0f;
        float width = height * aspectRatio;

        return Matrix4x4.CreateOrthographic(width, height, -1.0f, 1.0f);
    }
}
