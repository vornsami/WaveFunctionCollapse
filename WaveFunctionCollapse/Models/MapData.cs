using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveFunctionCollapse.Models
{
    public class MapData
    {
        private int[,] map;
        private Color[] colors;
        public MapData(int[,] map) {
            this.map = map;
            colors = [ Colors.White ];
        }
        public MapData(int[,] map, Color[] colors)
        {
            this.map = map;
            this.colors = colors;
        }

        public int[,] GetMap() { return map; }
        public Color[] GetColors() { return colors;}
        public void SetColors(Color[] colors) {  this.colors = colors;}
        public int GetWidth() {  return map.GetLength(0); }
        public int GetHeight() { return map.GetLength(1); }
    }
}
