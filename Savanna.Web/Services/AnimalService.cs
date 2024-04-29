using Common.Interfaces;

namespace Savanna.Web.Services
{
    public class AnimalService
    {
        private List<IAnimal> _animals = new List<IAnimal>();

        public void AddAnimal(IAnimal animal)
        {
            _animals.Add(animal);
        }

        public List<IAnimal> GetAnimals()
        {
            return _animals;
        }
    }
}
