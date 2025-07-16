using GamerApp.Pages;

namespace GamerApp
{

    public partial class MainPage : ContentPage
    {
        public MainPage() { InitializeComponent(); }
        private async void OnStartClicked(object sender, EventArgs e)
        {
            if (int.TryParse(txtQuantidade.Text, out int quantidade) &&
                double.TryParse(txtVelocidade.Text, out double velocidade))
            {
                await Navigation.PushAsync(new GamePage(quantidade, velocidade));
            }
        }
    }
}