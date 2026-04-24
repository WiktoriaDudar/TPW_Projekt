using System;
using System.Collections.ObjectModel;
using System.Timers;
using Model;
using Data;

namespace ViewModel
{
    public class ViewModel : IViewModel
    {
        private readonly IModelAPI model;
        private readonly System.Timers.Timer timer;

        public ObservableCollection<IBall> Balls { get; } = new ObservableCollection<IBall>();

        private double width;
        private double height;

        public ViewModel()
        {
            model = new ModelAPI();

            timer = new System.Timers.Timer(16);
            timer.Elapsed += (s, e) => Update();
        }

        public void Start(int count, double width, double height)
{
    this.width = width;
    this.height = height;

    model.Logic.GenerateBalls(count, width, height);

    Balls.Clear();
    foreach (var ball in model.Logic.Balls)
    {
        Balls.Add(ball);
    }

    timer.Start();
}


        public void Update()
        {
            model.Logic.UpdatePositions(width, height);
        }
    }
}
