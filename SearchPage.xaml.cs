namespace journal2;

public partial class SearchPage : ContentPage
{
    public SearchPage()
    {
        InitializeComponent();
    }

    private async void OnSearchClicked(object sender, EventArgs e)
    {
        var query = SearchEntry.Text?.Trim();

        if (string.IsNullOrEmpty(query))
        {
            await DisplayAlert("Search", "Enter a keyword", "OK");
            return;
        }

        // Replace this with your real database query
        var fakeResults = new List<string>
        {
            $"You searched for: {query}",
            "Result A",
            "Result B",
            "Result C"
        };

        ResultsList.ItemsSource = fakeResults;
    }
}
