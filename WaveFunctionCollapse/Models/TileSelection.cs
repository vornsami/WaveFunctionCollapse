using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveFunctionCollapse.Models
{
    internal class TileSelection
    {
        private readonly Color DEFAULT_COLOR = Colors.Orange;
        public ObservableCollection<MapTileInteraction> Interactions { get; set; } = [];
        public ObservableCollection<TileData> TileDataList { get; set; } = [];
        public int Size = 0;

        public TileSelection(int n) {
            Size = n;
            BuildFullList(n);
            BuildColorList(n);
        }
        private void BuildFullList(int n)
        {
            int combinations = n * n;

            for (int i = 0; i < combinations; i++)
            {
                int x = i % n;
                int y = i / n;
                (int, int) tup = (x, y);
                Interactions.Add(new MapTileInteraction { Position = tup, Color = DEFAULT_COLOR, BackgroundColor = DEFAULT_COLOR.WithAlpha(0.5f) });
            }
        }
        private void BuildColorList(int n)
        {
            for (int i = 0; i < n; i++)
            {
                MapTileInteraction[] checkboxColorList = Interactions
                    .Where(a => a.Position.y == i)
                    .ToArray();
                MapTileInteraction[] backgroundColorList = Interactions
                    .Where(a => a.Position.x == i)
                    .ToArray();
                TileDataList.Add(new TileData { TileColor = DEFAULT_COLOR, CheckboxColors = checkboxColorList, BackgroundColors = backgroundColorList, TileWeight = 10 });
            }
        }
    }
}
