using Mopups.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Input;
using WaveFunctionCollapse.Models;

namespace WaveFunctionCollapse.Views;

public partial class MapTileSettingsPage : ContentPage
{
    MapGenData generationData;
    TileSelection tileSelection;

    public ICommand UpdateCheckboxCommand { get; set; }
    public ICommand ColorSelector_Tapped { get; set; }

    public MapTileSettingsPage(MapGenData gendata)
    {
        InitializeComponent();
        generationData = gendata;
        tileSelection = new TileSelection(gendata.tileTypeCount);
        BindingContext = this;

        // Size label
        label.Text = tileSelection.Size + "";

        // Color selector
        colorTable.ItemsSource = tileSelection.TileDataList;
        ColorSelector_Tapped = new Command<TileData>(ColorSelectorTapped);

        // Tile table initialisation
        tileTypeTable.ItemsSource = tileSelection.Interactions;
        layout.Span = tileSelection.Size;
        UpdateCheckboxCommand = new Command<MapTileInteraction>(CheckboxChanged);
    }
    private void CheckboxChanged(MapTileInteraction obj)
    {
        if (obj != null)
        {
            // Needs to check/uncheck the mirror tile

            tileSelection.Interactions
                .Where(t => t.Position.x == obj.Position.y && t.Position.y == obj.Position.x)
                .First().IsChecked = obj.IsChecked;
        }
    }
    private void CheckNumeric(object sender, TextChangedEventArgs e)
    {
        string newText = e.NewTextValue;
        ((Entry)sender).Text = Regex.Replace(newText, "[^0-9.]", "");
    }
    private void ColorSelectorTapped(TileData obj)
    {
        Trace.WriteLine($"{obj.TileColor}");
        MopupService.Instance.PushAsync(new ColorpickPopup(obj));
    }

    private async void backButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(MapOptionPage)}");
    }

    private async void nextButton_Clicked(object sender, EventArgs e)
    {
        // Create color and tile weight list
        generationData.tileColors = new Color[generationData.tileTypeCount];
        generationData.weights = new int[generationData.tileTypeCount];

        for (int i = 0; i < generationData.tileColors.Length; i++)
        {
            TileData colsel = tileSelection.TileDataList[i];
            generationData.tileColors[i] = colsel.TileColor;
            generationData.weights[i] = (colsel.TileWeight > 0)? colsel.TileWeight : 1;
        }
        // Create interaction table
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