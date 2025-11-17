using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

namespace journal2{
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
        
        /* Event driven methods */
        /* this next method runs every time the main content page reloads 
         so override --> because void OnAppearning() exists in the ContentPage Class 
        we have new logic so you override it from content page. */
        protected override void OnAppearing()
        {
            base.OnAppearing(); // logic after this line runs after appearing. 
            var vm = BindingContext as FileViewModel;
            vm?.LoadFiles();
       
            if (vm?.Files.Any() == true)
                FileList.ScrollTo(vm.Files.Last());
        }
            
        /* This is an event handler method. you dont want other parts of the code touching it, so make it private
        async means that it is running out of step with the other parts of the code -- like it just happens in a new thread or something
        The .xaml file for button clicks has Clicked="OnNextPageClicked" and that's when it calls this method*/
        private async void OnNextPageClicked(object sender, EventArgs e)
        {
            /* await = pause untill navigation finishes doing 
             .PushAsync(...) put something into the navigation stack -- MAUI only displays the top page from the stack
            new WritingArea() constructs a new instance of WritingArea() -- so jump to that .xaml + .xaml.cs file
            
            you stay in WritingArea until in that class you do Navigation.PopAsync() -that pops that instance from the stack
            and the page instance is destroyed. State vanishes. so you should see a memory spike on this part.  
            oh this is where the comms come in -- Writing Area method now is apart of this memory*/
            var vm = BindingContext as FileViewModel;
            
            /*You can add in a fly out menu as an intermediate step here - and just return the type of page you want to make*/

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
}


