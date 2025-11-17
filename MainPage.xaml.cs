using System.Collections.ObjectModel;

namespace journal2;
public partial class MainPage : ContentPage // If you want to do a fly out page - you inherrit from flyout page
{
	public MainPage() // constructor method - runs when the page is created = __init__() method in python
	{
		InitializeComponent(); // loads everythng defined in .xaml and writes it up to .xaml.cs
		BindingContext = new FileViewModel(); // crerate context          
	}

	/*Data structures --> figure out how to put these into a models folder. */
	public class FileItem
	{
		public string Name { get; set; }
		public string Path { get; set; }
	}

	public class FileViewModel
	{
		// ObservableCollection -- we dont have lists in c#. so this is an iterable.
		public ObservableCollection<FileItem> Files { get; set; } = new();
		public int numfiles = 0;
		public FileViewModel() // constructor
		{
			LoadFiles(); // calls the load files method
		}
		public void LoadFiles()
		{
			Files.Clear(); // Make sure that the old file list is not being loaded too - start clean
			
			// get the app directory --> dir that the compiled app uses to store files... look to see if different from tmp and cache
			var dir = FileSystem.AppDataDirectory;
			var files = Directory.GetFiles(dir, "*.txt"); // gets all the files in the directory
			
			// iterate over all the files in the directory
			foreach (var f in files)
			{
				// add each file item to the Files object from ObservableCollection<FileItem>
				Files.Add(new FileItem // Like list.append() and we defined the FileItem data struture above
				{
					Name = Path.GetFileName(f),
					Path = f
				});
			}
			numfiles = 0 + Files.Count; // update the file count
			}
		}

	protected override void OnAppearing()
	{
		base.OnAppearing(); // logic after this line runs after appearing. 
		var vm = BindingContext as FileViewModel;
		vm?.LoadFiles();
	
		if (vm?.Files.Any() == true)
			FileList.ScrollTo(vm.Files.Last());
	}
		
	private async void OnNextPageClicked(object sender, EventArgs e)
	{
		var vm = BindingContext as FileViewModel;
		await Navigation.PushAsync(new WritingArea(inputFilePath:null, count:vm.numfiles));
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