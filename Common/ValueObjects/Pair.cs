namespace Common.ValueObjects;

public class Pair
{
    public int Animal1Id { get; }
    public int Animal2Id { get; }

    public Pair(int animal1Id, int animal2Id)
    {
        Animal1Id = animal1Id;
        Animal2Id = animal2Id;
    }

    public override bool Equals(object obj)
    {
        if (obj is Pair other)
        {
            return Animal1Id == other.Animal1Id && Animal2Id == other.Animal2Id ||
                   Animal1Id == other.Animal2Id && Animal2Id == other.Animal1Id;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Animal1Id.GetHashCode() ^ Animal2Id.GetHashCode();
    }
}
