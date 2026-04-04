using Logic;

namespace Model
{
    public interface IModelAPI
    {
        ILogicAPI Logic { get; }
    }
}
