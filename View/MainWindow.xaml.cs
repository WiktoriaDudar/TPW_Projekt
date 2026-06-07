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
            if (!int.TryParse(BallCountBox.Text, out int count) || count < 1 || count > 100)
            {
                MessageBox.Show("Podaj liczbę kulek od 1 do 100.");
                return;
            }

            double width = BallCanvas.ActualWidth;
            double height = BallCanvas.ActualHeight;

            _vm.Start(count, width, height);

            StartButton.IsEnabled = false;
        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _vm?.UpdateWindowSize(BallCanvas.ActualWidth, BallCanvas.ActualHeight);
        }
    }
}