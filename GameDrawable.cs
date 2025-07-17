using GamerApp.Models;
using Java.Security;
using Microsoft.Maui.Graphics.Platform;
using System.IO;
using System.Threading.Tasks;
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
            // Images will be loaded in the InitializeImagesAsync method
        }

        public async Task InitializeImagesAsync()
        {
            _imgRock = await LoadImage("Resources.Images.pedra.png");
            _imgPaper = await LoadImage("Resources.Images.papel.png");
            _imgScissors = await LoadImage("Resources.Images.tesoura.png");
        }

        private async Task<IImage> LoadImage(string filename)
        {
            try
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync(filename);
                return PlatformImage.FromStream(stream);

            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine($"Error loading image {filename}: {ex.Message}");
                return null;
            }
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