using Common.ValueObjects;

namespace Common.Interfaces;

public interface IGameField
{
    void SetState(int x, int y, FieldCell state);
    FieldCell GetState(int x, int y);
    void Initialize(FieldCell initialState);
    int Width { get; }
    int Height { get; }
}
