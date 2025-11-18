using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace journal2.Services
{
    // A simple class to hold file metadata
    public class FileItem
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }

    // Interface defines the contract for the service
    public interface IFileExplorer
    {
        ObservableCollection<FileItem> Files { get; set; }
        Task GetTextFiles();
        Task CreateSampleFile(string fileName, string content);
    }
    
    // File Explorer service implementation
    public class FileExplorer : IFileExplorer
    {
        // Data structure to hold the list of files, bound to the UI
        public ObservableCollection<FileItem> Files { get; set; } = new();

        /// <summary>
        /// Scans the application data directory for all files matching the .txt extension.
        /// </summary>
        public async Task GetTextFiles()
        {
            // Ensure the UI is updated on the main thread when clearing
            MainThread.BeginInvokeOnMainThread(() => Files.Clear());

            // Get the app's persistent storage directory
            var dir = FileSystem.AppDataDirectory;
            
            // Gets all file paths in the directory that end with .txt
            // Note: Directory.GetFiles is synchronous and non-blocking here as it's a file system operation.
            var files = Directory.GetFiles(dir, "*.txt"); 

            // Add the new files to the ObservableCollection
            MainThread.BeginInvokeOnMainThread(() =>
            {
                foreach (var f in files)
                {
                    Files.Add(new FileItem
                    {
                        Name = Path.GetFileName(f),
                        Path = f
                    });
                }
            });
        }
        
        /// <summary>
        /// Helper function to create a new text file for testing purposes.
        /// </summary>
        public async Task CreateSampleFile(string fileName, string content)
        {
            var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);
            await File.WriteAllTextAsync(filePath, content);
        }
    }
}