using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using journal2.Services;
namespace journal2;
    
public partial class MainPage : ContentPage
{
	private readonly IFileExplorer _explorer;

	public MainPage(IFileExplorer explorer) 
	{
		InitializeComponent(); 
		_explorer = explorer;
		Task.Run(async () => await _explorer.GetTextFilesAsync());
		BindingContext = _explorer;

		       
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing(); // logic after this line runs after appearing. 
		Task.Run(async () => await _explorer.GetTextFilesAsync());	
		FileList.ScrollTo(_explorer.Files.Last());

		
	
		// if (vm?.Files.Any() == true)
		// 	FileList.ScrollTo(vm.Files.Last());


	}
		
	private async void OnNextPageClicked(object sender, EventArgs e)
	{
		// var vm = BindingContext as FileViewModel;
		// await Navigation.PushAsync(new WritingArea(inputFilePath:null, count:vm.numfiles));
	}

	private async void OnFileSelected(object sender, SelectionChangedEventArgs e)
	{
		// // we never changed the binding context so this will pull the file item from file view model
		// var selected = e.CurrentSelection.FirstOrDefault() as FileItem;
		// if (selected == null) return;

		// // Pass the path to WritingArea
		// await Navigation.PushAsync(new WritingArea(inputFilePath: selected.Path));

		// Console.WriteLine($"Opening file: {selected.Path}");

		// ((CollectionView)sender).SelectedItem = null;
	}
}