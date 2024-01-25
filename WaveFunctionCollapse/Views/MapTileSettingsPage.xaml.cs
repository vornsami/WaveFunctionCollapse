using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WaveFunctionCollapse.Models;

namespace WaveFunctionCollapse.Views;

public partial class MapTileSettingsPage : ContentPage
{
    MapGenData generationData;
    TileSelection tileSelection;

    public ICommand UpdateThisItemCommand { get; set; }

    public MapTileSettingsPage(MapGenData gendata)
    {
        InitializeComponent();
        generationData = gendata;

        tileSelection = new TileSelection(gendata.tileTypeCount);
        
        tileTypeTable.ItemsSource = tileSelection.Interactions;
        label.Text = tileSelection.Size + "";
        layout.Span = tileSelection.Size;

        UpdateThisItemCommand = new Command<MapTileInteraction>(CheckboxChanged);
    }
    private void CheckboxChanged(MapTileInteraction obj)
    {
        if (obj != null && obj.IsChecked)
        {
            // Needs to check/uncheck the mirror tile
        }
    }

    private async void backButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(MapOptionPage)}");
    }

    private async void nextButton_Clicked(object sender, EventArgs e)
    {
        generationData.tileColors = new Color[generationData.tileTypeCount];

        generationData.tileColors = [Colors.Red, Colors.Blue, Colors.Green, Colors.Cyan]; // Placeholder
        /*for (int i = 0; i < generationData.tileColors.Length; i++)
        {
            generationData.tileColors[i] = Colors.Red;
        }*/

        for (int i = 0; i < tileSelection.Interactions.Count; i++)
        {
            MapTileInteraction mti = tileSelection.Interactions[i];
            if (!mti.IsChecked) 
                continue;

            (int x, int y) = mti.Position;
            
            generationData.tileData[x] |= 1 << y;
            generationData.tileData[y] |= 1 << x;
        }

        await Navigation.PushAsync(new MapGenerationPage(generationData));
    }
}