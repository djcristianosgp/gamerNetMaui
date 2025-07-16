using GamerApp.Models;
using Microsoft.Maui.Graphics.Platform;
using IImage = Microsoft.Maui.Graphics.IImage;

namespace GamerApp
{
    public class GameDrawable : IDrawable
    {
        private List<GameObject> _objetos;
        private IImage _imgRock, _imgPaper, _imgScissors;
        public GameDrawable(List<GameObject> objetos)
        {
            _objetos = objetos;

            _imgRock = LoadImage("Resources\\Images\\pedra.png");
            _imgPaper = LoadImage("Resources\\Images\\papel.png");
            _imgScissors = LoadImage("Resources\\Images\\tesoura.png");
        }

        private IImage LoadImage(string filename)
        {
            var stream = FileSystem.OpenAppPackageFileAsync(filename).Result;
            return PlatformImage.FromStream(stream);
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            foreach (var obj in _objetos)
            {
                IImage image = obj.Tipo switch
                {
                    "Pedra" => _imgRock,
                    "Papel" => _imgPaper,
                    "Tesoura" => _imgScissors,
                    _ => null
                };

                if (image != null)
                {
                    canvas.DrawImage(image,
                        (float)obj.PosX, (float)obj.PosY,
                        32, 32);
                }
            }
        }
    }
}
