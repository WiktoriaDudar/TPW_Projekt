using Data;
using Model;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace ViewModel
{
    public class ViewModel : IViewModel
    {
        private readonly IModelAPI model;
        private readonly Dispatcher _dispatcher;

        public ObservableCollection<IBall> Balls { get; } = new ObservableCollection<IBall>();

        public ViewModel(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            model = new ModelAPI();
        }

        public void Start(int count, double width, double height)
        {
            model.SetWindowSize(width, height);
            model.Logic.GenerateBalls(count);

            Balls.Clear();

            foreach (var ball in model.Logic.Balls)
            {
                ball.NewPositionNotification += OnBallPositionChanged;
                Balls.Add(ball);
            }
        }

        private void OnBallPositionChanged(object? sender, IVector velocity)
        {
            if (_dispatcher.HasShutdownStarted || _dispatcher.HasShutdownFinished)
                return;

            _dispatcher.BeginInvoke(() => { });
        }

        public void UpdateWindowSize(double width, double height)
        {
            model.SetWindowSize(width, height);
        }
    }
}
