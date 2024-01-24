using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using WaveFunctionCollapse.Models;

namespace WaveFunctionCollapse.Views;

public partial class MapTileSettingsPage : ContentPage
{
    MapGenData generationData;
    TileSelection tileSelection;

    public MapTileSettingsPage(MapGenData gendata)
    {
        InitializeComponent();
        generationData = gendata;

        tileSelection = new TileSelection(gendata.tileTypeCount);
        
        tileTypeTable.ItemsSource = tileSelection.FullList;
        tileTypeTable.SelectedItem = tileSelection.SelectedList;

        label.Text = tileSelection.Size + "";
        layout.Span = tileSelection.Size;
    }
    private async void backButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(MapOptionPage)}");
    }

    private async void nextButton_Clicked(object sender, EventArgs e)
    {
        for (int i = 0;i < tileSelection.SelectedList.Count; i++)
        {
            (int x, int y) pos = tileSelection.SelectedList[i];
            
            generationData.tileData[pos.x] |= 1 << pos.y;
            generationData.tileData[pos.y] |= 1 << pos.x;
        }

        await Navigation.PushAsync(new MapGenerationPage(generationData));
    }

    private void tileTypeTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try
        {
            string a = e.CurrentSelection.First().GetType().ToString();
            (int x, int y) comp = (1, 2);

            if (!e.CurrentSelection.First().GetType().Equals(comp.GetType()))
                return;
            tileSelection.SelectedList = e.CurrentSelection.ToList().ConvertAll(o => ((int x, int y))o);
        } catch (Exception eX)
        {
            Trace.WriteLine(eX);
        }
        
    }
}