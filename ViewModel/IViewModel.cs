using System.Collections.ObjectModel;
using Data;

namespace ViewModel
{
    public interface IViewModel
    {
        ObservableCollection<IBall> Balls { get; }
        void Start(int count, double width, double height);
        void UpdateWindowSize(double width, double height);
    }
}
