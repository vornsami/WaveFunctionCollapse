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
        public MapData(int[,] map) {
            this.map = map;
        }

        public int[,] getMap() { return map; }
        public int getWidth() {  return map.GetLength(0); }

        public int getHeight() { return map.GetLength(1); }
    }
}
