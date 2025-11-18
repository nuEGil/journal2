using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using journal2.Services;

namespace journal2{


	public partial class MainPage : ContentPage 
    {
        private readonly IFileExplorer _explorer;

        public MainPage(IFileExplorer explorer) // constructor method - runs when the page is created = __init__() method in python
        {
            InitializeComponent(); // loads everythng defined in .xaml and writes it up to .xaml.cs
            _explorer = explorer;
            // BindingContext = new FileViewModel(); // crerate context          
        }
        
        protected override void OnAppearing()
        {
            base.OnAppearing(); // logic after this line runs after appearing. 
            _explorer.GetTextFiles();
       
            if (_explorer.Files.Any() == true)
                FileList.ScrollTo(_explorer.Files.Last());
        }
            
        private async void OnNextPageClicked(object sender, EventArgs e)
        {
            // var vm = BindingContext as FileViewModel;
        
            await Navigation.PushAsync(new WritingArea(inputFilePath:null, count:_explorer.Files.Count));

        }
       
        private async void OnFileSelected(object sender, SelectionChangedEventArgs e)
        {
            // we never changed the binding context so this will pull the file item from file view model
            var selected = e.CurrentSelection.FirstOrDefault() as FileItem;
            if (selected == null) return;

            // Pass the path to WritingArea
            await Navigation.PushAsync(new WritingArea(inputFilePath: selected.Path));

            Console.WriteLine($"Opening file: {selected.Path}");

            ((CollectionView)sender).SelectedItem = null;
        }
    }
}


