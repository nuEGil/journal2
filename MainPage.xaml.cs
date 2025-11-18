using journal2.ViewModels;
using journal2.Services;

namespace journal2
{
    public partial class MainPage : ContentPage
    {
        private readonly MainPageViewModel _vm;

        public MainPage(MainPageViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            BindingContext = _vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await _vm.LoadFiles();

            if (_vm.Files.Any())
                FileList.ScrollTo(_vm.Files.Last());
        }

        private async void OnNextPageClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(
                new WritingArea(inputFilePath: null, count: _vm.Files.Count));
        }

        private async void OnFileSelected(object sender, SelectionChangedEventArgs e)
        {
            var selected = e.CurrentSelection.FirstOrDefault() as FileItem;
            if (selected == null) return;

            await Navigation.PushAsync(new WritingArea(selected.Path));

            Console.WriteLine($"Opening file: {selected.Path}");

            ((CollectionView)sender).SelectedItem = null;
        }
    }
}
