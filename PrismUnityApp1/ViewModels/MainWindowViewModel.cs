using Prism.Mvvm;

namespace TicTacToe.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Noughts and Crosses";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {

        }
    }
}
