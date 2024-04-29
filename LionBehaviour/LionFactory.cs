
using Common.Interfaces;

namespace LionBehaviour
{
    public class LionFactory : IAnimalFactory
    {
        public string? Species => new Lion().Species;
        public char Symbol => new Lion().Symbol;
        public string? Icon => new Lion().Icon;

        public IAnimal Create()
        {
            var lion = new Lion
            {
                Health = Common.Constants.Constant.InitialHealth,
                Age = Common.Constants.Constant.InitialAge
            };
            return lion;
        }
    }
}
