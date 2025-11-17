using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace journal2.Services
{
   public struct FileItem
        {
            public string Name { get; set;}
            public string Path { get; set;}
        }

    // interface just says hey these are the functions I expect to see
    public interface IFileExplorer
    {
        Task GetTextFilesAsync();
       
    }
    // File Explorer implements a class that is IFileExplorer
    public class FileExplorer : IFileExplorer
    {    // data structure to groupfile name and path -- good for database stuff later
        
        
        public ObservableCollection<FileItem> Files { get; set; } = new();
        
        public async Task GetTextFilesAsync()
        {
            Files.Clear(); // Make sure that the old file list is not being loaded too - start clean
			
			// get the app directory --> dir that the compiled app uses to store files... look to see if different from tmp and cache
			var dir = FileSystem.AppDataDirectory;
			var files = Directory.GetFiles(dir, "*.txt"); // gets all the files in the directory
			
			// iterate over all the files in the directory
			foreach (var f in files)
			{			
				Files.Add(new FileItem 
				{
					Name = Path.GetFileName(f),
					Path = f
				});
			}
			}
        }    
}
    