using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveFunctionCollapse.Models
{
    internal class AllMaps
    {
        public ObservableCollection<MapInfo> Maps { get; set; } = new ObservableCollection<MapInfo>();
    }
}
