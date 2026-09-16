using Valour.Engine;

namespace Valour.Game;

internal class ValourGame : IGame
{
    private Shader _shader;
    private Mesh _mesh;

    public void Load(Renderer renderer)
    {
        string vertPath = Path.Combine(AppContext.BaseDirectory, "resources", "shader.vert");
        string fragPath = Path.Combine(AppContext.BaseDirectory, "resources", "shader.frag");
        
        string vertSource = File.ReadAllText(vertPath);
        string fragSource = File.ReadAllText(fragPath);

        _shader = renderer.CreateShader(vertSource, fragSource);

        Vertex[] vertices =
        [
            new(new(-0.5f, -0.5f), new(0.0f, 0.0f)),
            new(new( 0.5f, -0.5f), new(1.0f, 0.0f)),
            new(new( 0.0f,  0.5f), new(0.5f, 1.0f)),
        ];

        uint[] indices = [0, 1, 2];

        _mesh = renderer.CreateMesh(vertices, indices);
    }

    public void Update(double deltaTime)
    {

    }

    public void Render(Renderer renderer)
    {
        renderer.Draw(_mesh, _shader);
    }

    public void Unload()
    {
        _shader.Dispose();
        _mesh.Dispose();
    }
}