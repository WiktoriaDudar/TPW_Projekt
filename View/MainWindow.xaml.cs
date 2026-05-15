using System;
using System.Windows;
using ViewModel;

namespace View
{
    public partial class MainWindow : Window
    {
        private readonly ViewModel.ViewModel _vm;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = new ViewModel.ViewModel(Application.Current.Dispatcher);

            _vm = (ViewModel.ViewModel)DataContext;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            double width = BallCanvas.ActualWidth;
            double height = BallCanvas.ActualHeight;

            if (int.TryParse(BallCountBox.Text, out int count))
            {
                _vm.Start(count, width, height);
            }
            else
            {
                MessageBox.Show("Wpisz poprawną liczbę.");
            }

            StartButton.IsEnabled = false;
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
