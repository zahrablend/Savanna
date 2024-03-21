using Common.Interfaces;

namespace LionBehaviour
{
    public class LionFactory : IAnimalFactory
    {
        public char Symbol => new Lion().Symbol;

        public IAnimal Create()
        {
            return new Lion();
        }
    }
}
