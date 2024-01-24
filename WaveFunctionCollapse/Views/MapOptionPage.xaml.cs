namespace WaveFunctionCollapse.Views;

using System.Text.RegularExpressions;
using WaveFunctionCollapse.Models;

public partial class MapOptionPage : ContentPage
{
	public MapOptionPage()
	{
		InitializeComponent();
	}

    private void CheckNumeric(object sender, TextChangedEventArgs e)
	{
		string newText = e.NewTextValue;
		((Entry)sender).Text = Regex.Replace(newText, "[^0-9.]", "");
	}

    private async void backButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(MainPage)}");
    }

    private async void nextButton_Clicked(object sender, EventArgs e)
    {
        int tileTypeCount = int.Parse(tiletypecount.Text);
        MapGenData data = new()
        {
            tileTypeCount = tileTypeCount,
            mapSizeX = int.Parse(mapsizex.Text),
            mapSizeY = int.Parse(mapsizey.Text),
            tileData = new int[tileTypeCount]
        };

        await Navigation.PushAsync(new MapTileSettingsPage(data));
    }
}