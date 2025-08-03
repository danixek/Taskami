using System.Text;
using Taskami.Views;
using System.Windows;

namespace Taskami
{
    /// <summary>
    /// Interaction logic for Taskami.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShowTodoView(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new TodoView();
        }

        private void ShowDiaryView(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new DiaryView();
        }

        private void ShowPomodoroView(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new PomodoroView();
        }
    }
}