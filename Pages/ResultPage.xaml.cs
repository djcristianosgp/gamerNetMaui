namespace GamerApp.Pages
{
    public partial class ResultPage : ContentPage
    {
        public ResultPage(string vencedor)
        {
            InitializeComponent();
            lblVencedor.Text = $"{vencedor}";
        }
        private async void OnReiniciar(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
    }
}