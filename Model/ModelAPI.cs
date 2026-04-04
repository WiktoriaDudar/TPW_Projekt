using Data;
using Logic;

namespace Model
{
    public class ModelAPI : IModelAPI
    {
        public ILogicAPI Logic { get; }

        public ModelAPI()
        {
            var repository = new DataRepository();
            Logic = new LogicAPI(repository);
        }
    }
}

