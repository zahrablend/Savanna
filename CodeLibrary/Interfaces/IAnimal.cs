namespace CodeLibrary.Interfaces;

public interface IAnimal
{
    int X { get; set; }
    int Y { get; set; }

    void MoveAnimal(IAnimal[,] gameField, int fieldHeight, int fieldWidth);
}
