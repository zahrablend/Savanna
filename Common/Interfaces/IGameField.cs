namespace Common.Interfaces;

public interface IGameField
{
    void SetState(int x, int y, object state);
    object GetState(int x, int y);
    void Initialize(object initialState);
}
