using System.Drawing;
using WaveFunctionCollapse.Models;
using Color = Microsoft.Maui.Graphics.Color;

namespace WaveFunctionCollapse.Views;

public partial class MapPage : ContentPage
{
    readonly int MAP_SCALE = 3;
    MapData map;

    public MapPage(MapData map)
	{
		InitializeComponent();
        canvas.HeightRequest = map.getHeight() * MAP_SCALE;
        canvas.WidthRequest = map.getWidth() * MAP_SCALE;
        MapCanvas mapCanvas = (MapCanvas) canvas.Drawable;
        this.map = map;
        mapCanvas.setMap(map);
    }

    
    private async void backButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }
}
public class MapCanvas : IDrawable
{
    readonly int MAP_SCALE = 3;
    MapData map;
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        if(map == null)
        {
            canvas.StrokeColor = Colors.Red;
            canvas.StrokeSize = 6;
            canvas.DrawLine(10, 10, 90, 100);
            return;
        }
        canvas.StrokeSize = 1;
        int[,] mapTiles = map.getMap();

        for (int x = 0; x < map.getWidth(); x++)
        {
            for (int y = 0; y < map.getHeight(); y++)
            {
                canvas.FillColor = getColor(mapTiles[x,y]);

                canvas.FillRectangle(x * MAP_SCALE, y * MAP_SCALE, MAP_SCALE, MAP_SCALE);

            }
        }
        
    }
    private Color getColor(int i)
    {
        return i switch
        {
            0 => Colors.Black,
            1 => Colors.Red,
            2 => Colors.Green,
            3 => Colors.Blue,
            4 => Colors.Cyan,
            5 => Colors.Yellow,
            6 => Colors.DeepPink,
            7 => Colors.Gray,
            8 => Colors.Beige,
            9 => Colors.DarkGreen,
            10 => Colors.DarkBlue,
            _ => Colors.White,
        };
    }
    public void setMap(MapData map)
    {
        this.map = map;
    }
}