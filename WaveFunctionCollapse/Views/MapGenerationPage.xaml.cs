namespace WaveFunctionCollapse.Views;

using System.Runtime.CompilerServices;
using WaveFunctionCollapse.Models;

public partial class MapGenerationPage : ContentPage
{
    MapGenData GenerationData;
    public MapGenerationPage(MapGenData gendata)
    {
        InitializeComponent();

        GenerationData = gendata;
    }

    protected override void OnAppearing()
    {
        Dispatcher.Dispatch(async () =>
        {
            runGenerationAndRedirect();
        });
    }
    
    private async void runGenerationAndRedirect() {
        MapData map = MapGeneration.GenerateMap(GenerationData);
        await Navigation.PushAsync(new MapPage(map));
    }
}