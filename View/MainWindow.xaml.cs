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
            _vm = (ViewModel.ViewModel)DataContext;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            double width = BallCanvas.ActualWidth;
            double height = BallCanvas.ActualHeight;

            if (width == 0 || height == 0)
            {
                width = 800;
                height = 500;
            }

            _vm.Start(20, width, height);
        }
    }
}
