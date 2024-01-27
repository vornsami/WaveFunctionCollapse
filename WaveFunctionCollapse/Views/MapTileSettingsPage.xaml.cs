using Mopups.Services;
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
        colorTable.ItemsSource = tileSelection.TileColors;
        ColorSelector_Tapped = new Command<ColorSelector>(ColorSelectorTapped);

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
    private void ColorSelectorTapped(ColorSelector obj)
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
        generationData.tileColors = new Color[generationData.tileTypeCount];

        for (int i = 0; i < generationData.tileColors.Length; i++)
        {
            ColorSelector colsel = tileSelection.TileColors[i];
            generationData.tileColors[i] = colsel.TileColor;
        }

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