using Microsoft.Maui.Storage;
using journal2.Services;
using System.IO;

namespace journal2;

// Partial because it pairs with WritingArea.xaml
public partial class WritingArea : ContentPage
{
    // ------------------------
    // Injected Services
    // ------------------------
    private readonly IDatabaseService _db;
    private readonly IKeywordService _keywords;

    // ------------------------
    // Page State
    // ------------------------
    public int gcount = 0;
    public string FilePath { get; private set; } 
    public string FileText { get; private set; }

    private const string default_fname = "note_";
    private const string extension = ".txt";

    // ------------------------
    // Constructor
    // ------------------------
    public WritingArea(
        IDatabaseService db,
        IKeywordService keywords,
        string inputFilePath = null,
        int count = 0)
    {
        InitializeComponent();
        
        _db = db;
        _keywords = keywords;

        gcount = count;

        // Existing file → load contents
        if (!string.IsNullOrEmpty(inputFilePath) && File.Exists(inputFilePath))
        {
            FilePath = inputFilePath;
            FileText = File.ReadAllText(inputFilePath);

            myEditor.Text = FileText;
            myEntry.Text = Path.GetFileNameWithoutExtension(FilePath);
        }
        else
        {
            // New file
            myEntry.Text = default_fname + gcount;
            FilePath = Path.Combine(FileSystem.AppDataDirectory, $"{default_fname}{gcount}{extension}");
        }
    }

    // ------------------------
    // File Name Changed
    // ------------------------
    private void OnNameChanged(object sender, TextChangedEventArgs e)
    {
        var input = e.NewTextValue;

        string fileName = string.IsNullOrWhiteSpace(input)
                            ? default_fname + extension
                            : input + extension;

        FilePath = Path.Combine(FileSystem.AppDataDirectory, fileName);
    }

    // ------------------------
    // Save Button Click
    // ------------------------
    private async void OnSaveClicked(object sender, EventArgs e)
    {
        var text = myEditor.Text;

        if (string.IsNullOrWhiteSpace(text))
        {
            await DisplayAlert("Empty", "Cannot save an empty note.", "OK");
            return;
        }

        // 1. Write the text file
        File.WriteAllText(FilePath, text);

        // 2. Ensure file exists in DB (files table)
        var filename = Path.GetFileName(FilePath);
        int fileId = await _db.GetOrCreateFileId(filename);

        // 3. Extract keywords from text
        var foundKeywords = _keywords.ExtractKeywords(text);

        // 4. Insert each keyword match into DB
        foreach (var keyword in foundKeywords)
        {
            await _db.InsertKeywordEntry(fileId, keyword, text);
        }

        await DisplayAlert("Saved", $"Indexed {foundKeywords.Length} keyword(s).", "OK");
    }

    // ------------------------
    // Back Button
    // ------------------------
    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    // ------------------------
    // Delete Button
    // ------------------------
    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (File.Exists(FilePath))
            File.Delete(FilePath);

        await DisplayAlert("Deleted", "Note deleted.", "OK");
        await Navigation.PopAsync();
    }
}
