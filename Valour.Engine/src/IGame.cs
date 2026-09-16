namespace Valour.Engine;

public interface IGame
{
    void Load(Renderer renderer);
    void Update(double deltaTime);
    void Render(Renderer renderer);
    void Unload(); // called before GL teardown
}