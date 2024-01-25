using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveFunctionCollapse.Models
{
    internal class TileSelection
    {
        public ObservableCollection<MapTileInteraction> Interactions { get; set; } = [];

        public int Size = 0;

        public TileSelection(int n) {
            Size = n;
            BuildFullList(n);
        }
        private void BuildFullList(int n)
        {
            int combinations = n * n;

            for (int i = 0; i < combinations; i++)
            {
                int x = i % n;
                int y = i / n;
                (int, int) tup = (x, y);
                Interactions.Add(new MapTileInteraction { Position = tup });
            }
        }
    }
}
