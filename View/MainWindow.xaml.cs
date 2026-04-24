using System;
using System.Windows;
using System.Windows.Threading;
using ViewModel;

namespace View
{
    public partial class MainWindow : Window
    {
        private readonly ViewModel.ViewModel _vm;
        private readonly DispatcherTimer _timer;

        public MainWindow()
        {
            InitializeComponent();
            _vm = (ViewModel.ViewModel)DataContext;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(16);
            _timer.Tick += (s, e) => _vm.Update(); 
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            double width = 500;
            double height = 500;

            if (int.TryParse(BallCountBox.Text, out int count))
            {
                _vm.Start(count, width, height);
                _timer.Start(); 
            }
            else
            {
                MessageBox.Show("Wpisz poprawną liczbę.");
            }
        }
    }
}
