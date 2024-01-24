using WaveFunctionCollapse.Views;

namespace WaveFunctionCollapse
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void ToMapCreation(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"{nameof(MapOptionPage)}");
        }
    }

}
