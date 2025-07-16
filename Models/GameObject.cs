namespace GamerApp.Models
{
    public class GameObject
    {
        public string Tipo { get; set; }
        public double PosX { get; set; }
        public double PosY { get; set; }
        public double Velocidade { get; set; }
        public double DirecaoX { get; set; }
        public double DirecaoY { get; set; }
        public bool Ativo { get; set; } = true;
        public double DistanciaPara(GameObject outro)
        {
            double dx = this.PosX - outro.PosX;
            double dy = this.PosY - outro.PosY;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }

}