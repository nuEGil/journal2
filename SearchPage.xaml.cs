using journal2.Services;
using System.Collections.ObjectModel;

namespace journal2;

public partial class SearchPage : ContentPage
{
    private readonly IDatabaseService _db;

    public ObservableCollection<string> Results { get; set; } = new();

    public SearchPage(IDatabaseService db)
    {
        InitializeComponent();
        _db = db;

        BindingContext = this;
    }

    private async void OnSearchClicked(object sender, EventArgs e)
    {
        Results.Clear();

        string keyword = KeywordEntry.Text?.Trim().ToLower();

        if (string.IsNullOrWhiteSpace(keyword))
        {
            await DisplayAlert("Error", "Please enter a keyword.", "OK");
            return;
        }

        // hit the DB
        var matches = await _db.FindFilesByKeyword(keyword);

        if (matches.Count == 0)
        {
            await DisplayAlert("No Results", "No notes matched that keyword.", "OK");
            return;
        }

        // display in the UI
        foreach (var f in matches)
            Results.Add(f);
    }

    private async void OnResultSelected(object sender, SelectionChangedEventArgs e)
    {
        var filename = e.CurrentSelection.FirstOrDefault() as string;
        if (filename == null) return;

        // full file path
        var fullPath = Path.Combine(FileSystem.AppDataDirectory, filename);

        await Navigation.PushAsync(new WritingArea(fullPath));

        ((CollectionView)sender).SelectedItem = null;
    }
}
