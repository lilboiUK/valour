using Silk.NET.Input;

namespace Valour.Engine;

public interface IGame
{
    void Load(Renderer renderer);
    void Update(double deltaTime, IInputContext input);
    void Render(Renderer renderer);
    void Unload(); // called before GL teardown
}