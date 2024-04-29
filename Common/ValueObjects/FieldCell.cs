namespace Common.ValueObjects;

public class FieldCell
{
    public object? State { get; set; }
    public bool IsEmpty => State == null;
}
