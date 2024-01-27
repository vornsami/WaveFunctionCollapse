using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WaveFunctionCollapse.Models
{
    public class MapTileInteraction : INotifyPropertyChanged
    {

        public (int x, int y) Position { get; set; }
        private bool _isChecked;
        private Color _color;
        private Color _backgroundColor;
        public Color Color 
        {
            set { SetProperty(ref _color, value); }
            get { return _color; }
        }
        public Color BackgroundColor
        {
            set { SetProperty(ref _backgroundColor, value); }
            get { return _backgroundColor; }
        }
        public bool IsChecked
        {
            set { SetProperty(ref _isChecked, value); }
            get { return _isChecked; }
        }

        bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Object.Equals(storage, value))
                return false;
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
