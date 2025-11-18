using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using journal2.Services;

namespace journal2.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private readonly IFileExplorer _explorer;

        public ObservableCollection<FileItem> Files => _explorer.Files;

        public ICommand RefreshCommand { get; }
        public ICommand CreateSampleCommand { get; }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(); }
        }

        public MainPageViewModel(IFileExplorer explorer)
        {
            _explorer = explorer;

            RefreshCommand = new Command(async () => await LoadFiles());
            CreateSampleCommand = new Command(async () =>
                await _explorer.CreateSampleFile($"sample-{DateTime.Now.Ticks}.txt", "Hello world"));

            _ = LoadFiles();
        }

        public async Task LoadFiles()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                await _explorer.GetTextFiles();
            }
            finally
            {
                IsBusy = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
