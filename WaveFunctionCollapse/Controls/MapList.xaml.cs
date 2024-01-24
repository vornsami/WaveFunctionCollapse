namespace WaveFunctionCollapse.Controls;

public partial class MapList : ContentView
{
	public MapList()
	{
		InitializeComponent();

        BindingContext = new Models.AllMaps();
    }
    private async void mapsCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        throw new NotImplementedException();
    }
}