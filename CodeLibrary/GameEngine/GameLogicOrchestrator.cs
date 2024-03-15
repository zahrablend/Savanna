﻿using CodeLibrary.Constants;
using CodeLibrary.Interfaces;

namespace CodeLibrary.GameEngine;

public class GameLogicOrchestrator
{
    private IAnimal?[,] _gameField;
    private List<IAnimal> _animals;
    private readonly FieldDisplayer.FieldSize _fieldSize;
    private readonly FieldDisplayer _fieldDisplayer;
    private AnimalMover _animalMover;
    private HealthMetricCounter _healthMetricCounter;
    private AnimalRemover _animalRemover;
    private AnimalCreator _animalCreator;

    public GameLogicOrchestrator() { }

    public GameLogicOrchestrator(FieldDisplayer.FieldSize fieldSize, FieldDisplayer fieldDisplayer)
    {
        _fieldSize = fieldSize;
        _gameField = new IAnimal[_fieldSize.Height, _fieldSize.Width];
        _animals = new List<IAnimal>();
        _fieldDisplayer = fieldDisplayer;

        // Initialize the objects
        _animalMover = new AnimalMover(_gameField, fieldSize);
        _healthMetricCounter = new HealthMetricCounter(_gameField, fieldSize);
        _animalRemover = new AnimalRemover(_gameField, _animals);
        _animalCreator = new AnimalCreator(this);
    }

    public virtual void AddAnimal(IAnimal animal)
    {
        int x;
        int y;

        do
        {
            x = new Random().Next(_fieldSize.Height);
            y = new Random().Next(_fieldSize.Width);
        }
        while (_gameField[x, y] != null);

        animal.X = x;
        animal.Y = y;
        animal.Health = Constant.InitialHealth;
        _gameField[x, y] = animal;
        _animals.Add(animal);
    }

    public void PlayGame(IAnimal animal)
    {
        _animalMover.MoveAnimal(animal);
        _healthMetricCounter.DecreaseHealth(animal);
        _healthMetricCounter.InteractWith(animal);
        _animalRemover.RemoveAnimalOnDeath(animal);
        _animalCreator.CreateAnimalOnBirth();
    }

    public string DrawField => _fieldDisplayer.DrawField(_gameField, _fieldSize.Height, _fieldSize.Width);
    public List<IAnimal> GetAnimals => _animals;
    public IAnimal[,] GetGameField => _gameField;
}