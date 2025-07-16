using GamerApp.Models;
using Microsoft.Maui.Graphics;

namespace GamerApp.Pages
{
    public partial class GamePage : ContentPage
    {
        int _quantidade;
        double _velocidade;
        List<GameObject> _objetos = new();
        System.Timers.Timer _timer;
        GameDrawable _drawable;

        public GamePage(int quantidade, double velocidade)
        {
            InitializeComponent();
            _quantidade = quantidade;
            _velocidade = velocidade;

            _drawable = new GameDrawable(_objetos);
            canvas.Drawable = _drawable;
            canvas.Invalidate();

            StartGame();

        }
        private void StartGame()
        {
            var rand = new Random();
            var tipos = new[] { "Pedra", "Papel", "Tesoura" };
            for (int i = 0; i < _quantidade; i++)
            {
                _objetos.Add(new GameObject
                {
                    Tipo = tipos[rand.Next(tipos.Length)],
                    PosX = rand.NextDouble() * 300,
                    PosY = rand.NextDouble() * 400,
                    Velocidade = _velocidade,
                    DirecaoX = rand.NextDouble() * 2 - 1,
                    DirecaoY = rand.NextDouble() * 2 - 1
                });
            }
            _timer = new System.Timers.Timer(30);
            _timer.Elapsed += (_, __) => AtualizarMovimento();
            _timer.Start();
        }

        private void AtualizarMovimento()
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                foreach (var obj in _objetos)
                {
                    obj.PosX += obj.DirecaoX * obj.Velocidade * 10;
                    obj.PosY += obj.DirecaoY * obj.Velocidade * 10;

                    if (obj.PosX < 0 || obj.PosX > canvas.Width) obj.DirecaoX *= -1;
                    if (obj.PosY < 0 || obj.PosY > canvas.Height) obj.DirecaoY *= -1;
                }

                // Regras do jogo: colisão e eliminação
                double raio = 24; // Aproximado ao tamanho do emoji
                for (int i = 0; i < _objetos.Count; i++)
                {
                    for (int j = i + 1; j < _objetos.Count; j++)
                    {
                        var a = _objetos[i];
                        var b = _objetos[j];
                        if (!a.Ativo || !b.Ativo) continue;

                        double dx = a.PosX - b.PosX;
                        double dy = a.PosY - b.PosY;
                        double dist = Math.Sqrt(dx * dx + dy * dy);

                        if (dist < raio)
                        {
                            string vencedor = Vencedor(a.Tipo, b.Tipo);
                            if (vencedor == a.Tipo) b.Ativo = false;
                            else if (vencedor == b.Tipo) a.Ativo = false;
                        }
                    }
                }

                _objetos.RemoveAll(o => !o.Ativo);

                // Atualizar contagem na label
                int r = _objetos.Count(x => x.Tipo == "Pedra");
                int p = _objetos.Count(x => x.Tipo == "Papel");
                int s = _objetos.Count(x => x.Tipo == "Tesoura");
                lblStatus.Text = $"🪨 {r}  📄 {p}  ✂️ {s}";

                // Verifica fim automático do jogo
                var vivos = _objetos.Select(x => x.Tipo).Distinct().ToList();
                if (vivos.Count == 1)
                {
                    _timer.Stop();
                    await Navigation.PushAsync(new ResultPage($"Vencedor: {vivos[0]}"));
                    return;
                }

                canvas.Invalidate();
            });
        }

        private string Vencedor(string a, string b)
        {
            if (a == b) return a;
            return (a, b) switch
            {
                ("Pedra", "Tesoura") => "Pedra",
                ("Tesoura", "Papel") => "Tesoura",
                ("Papel", "Pedra") => "Papel",
                ("Tesoura", "Pedra") => "Pedra",
                ("Papel", "Tesoura") => "Tesoura",
                ("Pedra", "Papel") => "Papel",
                _ => a
            };
        }

        private async void OnFinalizar(object sender, EventArgs e)
        {
            _timer?.Stop();
            string vencedor = _objetos.GroupBy(o => o.Tipo)
                                       .OrderByDescending(g => g.Count())
                                       .FirstOrDefault()?.Key ?? "Ninguém";
            await Navigation.PushAsync(new ResultPage($"{vencedor}"));
        }
    }
}