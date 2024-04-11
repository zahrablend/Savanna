﻿using Common.Interfaces;

namespace Savanna.Web.Services;

public class AnimalGameFieldService : IGameField
{
    private IAnimal[,] _field;

    public AnimalGameFieldService(int width, int height)
    {
        _field = new IAnimal[width, height];
    }

    public void SetState(int x, int y, object state)
    {
        _field[y, x] = (IAnimal)state;
    }

    public object GetState(int x, int y)
    {
        return _field[y, x];
    }

    public void Initialize(object initialState)
    {
        
    }
}
