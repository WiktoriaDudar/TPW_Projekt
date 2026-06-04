using System;
using System.Windows;
using System.Windows.Media;
using ViewModel;

namespace View
{
    public partial class MainWindow : Window
    {
        private readonly ViewModel.ViewModel _vm;
        private readonly System.Diagnostics.Stopwatch _timer = new();

        public MainWindow()
        {
            InitializeComponent();

            DataContext = new ViewModel.ViewModel(Application.Current.Dispatcher);
            _vm = (ViewModel.ViewModel)DataContext;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(BallCountBox.Text, out int count) || count < 1 || count > 100)
            {
                MessageBox.Show("Podaj liczbę kulek od 1 do 100.");
                return;
            }

            double width = BallCanvas.ActualWidth;
            double height = BallCanvas.ActualHeight;

            _vm.Start(count, width, height);

            _timer.Restart();
            CompositionTarget.Rendering += UpdateTimer;

            StartButton.IsEnabled = false;
        }

        private void UpdateTimer(object? sender, EventArgs e)
        {
            TimerText.Text = $"Czas działania: {_timer.Elapsed.TotalSeconds:F2} s";
        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_vm != null)
            {
                _vm.UpdateWindowSize(BallCanvas.ActualWidth, BallCanvas.ActualHeight);
            }
        }
    }
}
