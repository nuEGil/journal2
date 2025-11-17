using Microsoft.Maui.Storage;
using System.IO;
namespace journal2;
// Partial because it is shared with the WritingArea.xaml
public partial class WritingArea : ContentPage // you could inherit from FlyoutPage here. 
{
    public int gcount = 0;
    public string FilePath { get; private set; } // filepath 
    public string FileText { get; private set; }  // text string to hold the file text 

    public string default_fname = $"note_";
    public string extension = ".txt";

    public WritingArea(string inputFilePath = null, int count = 0)
	{
		InitializeComponent(); // start the page
        gcount = count;
        if (!string.IsNullOrEmpty(inputFilePath) && File.Exists(inputFilePath))
        {
            FilePath = inputFilePath; // need this to see the file path in other methods
            FileText = File.ReadAllText(inputFilePath); // Read the file then 
            myEditor.Text = FileText; // set this
            myEntry.Text = Path.GetFileNameWithoutExtension(FilePath);
        }

        // set a default file name if it is empty
        if (string.IsNullOrEmpty(FilePath))
        {
            myEntry.Text = default_fname;
            FilePath = Path.Combine(FileSystem.AppDataDirectory, default_fname + $"{gcount}" + extension);
        }
       
    }
    // Leave this page
    private void OnNameChanged(object sender, TextChangedEventArgs e)
    {
        // sanitize name (no empty strings, no invalid file chars)
        string input = e.NewTextValue;
        string fileName;

        if (string.IsNullOrWhiteSpace(input))
        {
            fileName = default_fname+ extension;
        }
        else {
            fileName = input + extension;
        }

        FilePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
    // Save the file writing area
    private async void OnSaveClicked(object sender, EventArgs e)
    {
        // I do want to be able to overwrite the file 
        File.WriteAllText(FilePath, myEditor.Text);
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {

        if (File.Exists(FilePath))
        {
            File.Delete(FilePath);
        }
        await Navigation.PopAsync();


    }
}
