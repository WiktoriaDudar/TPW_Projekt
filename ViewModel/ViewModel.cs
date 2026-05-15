using Data;
using Model;
using System;
using System.Collections.ObjectModel;
using System.Timers;
using System.Windows;       

namespace ViewModel
{
    public class ViewModel : IViewModel
    {
        private readonly IModelAPI model;
        private readonly System.Timers.Timer timer;

        public ObservableCollection<IBall> Balls { get; } = new ObservableCollection<IBall>();

        public ViewModel()
        {
            model = new ModelAPI();

            timer = new System.Timers.Timer(16);
            timer.Elapsed += (s, e) => Update();
        }

        public void Start(int count, double width, double height)
        {
            model.SetWindowSize(width, height);

            model.Logic.GenerateBalls(count);

            Balls.Clear();
            foreach (var ball in model.Logic.Balls)
            {
                Balls.Add(ball);
            }

            timer.Start();
        }

        private void Update()
        {
            model.Logic.UpdatePositions();

            for (int i = 0; i < Balls.Count; i++)
            {
                Balls[i].X = model.GetBallX(i);
                Balls[i].Y = model.GetBallY(i);
            }
        }

        public void UpdateWindowSize(double width, double height)
        {
            model.SetWindowSize(width, height);
        }
    }
}
