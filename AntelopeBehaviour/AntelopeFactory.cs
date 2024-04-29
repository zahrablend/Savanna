using Common.Interfaces;

namespace AntelopeBehaviour;

public class AntelopeFactory : IAnimalFactory
{
    public string? Species => new Antelope().Species;
    public char Symbol => new Antelope().Symbol;
    public string? Icon => new Antelope().Icon;

    public IAnimal Create()
    {
        var antelope = new Antelope
        {
            Health = Common.Constants.Constant.InitialHealth,
            Age = Common.Constants.Constant.InitialAge,
        };
        return antelope;
    }
}
