using System;
using System.Collections.ObjectModel;
using System.Timers;
using Model;
using Data;

namespace ViewModel
{
    public class ViewModel : IViewModel
    {
        private readonly IModelAPI _model;
        private readonly System.Timers.Timer _timer;

        public ObservableCollection<IBall> Balls { get; } = new ObservableCollection<IBall>();

        public ViewModel()
        {
            _model = new ModelAPI();

            _timer = new System.Timers.Timer(16);
            _timer.Elapsed += (s, e) => Update();
        }

        public void Start(int count, double width, double height)
        {
            _model.Logic.GenerateBalls(count, width, height);

            Balls.Clear();
            foreach (var ball in _model.Logic.Balls)
            {
                Balls.Add(ball);
            }

            _timer.Start();
        }

        private void Update()
        {
            _model.Logic.UpdatePositions();
        }
    }
}
