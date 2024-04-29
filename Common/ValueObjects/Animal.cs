namespace Common.ValueObjects;

public abstract class Animal
{
    public int Id { get; protected set; }
    public abstract char Symbol { get; }
    public abstract string Icon { get; }

    public string GetRepresentation(bool useIcon)
    {
        return useIcon ? Icon : Symbol.ToString();
    }
}
