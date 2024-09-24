using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Threading;
using ZeroElectric.Vinculum;
using System.Reflection;

namespace net_rogue
{

    public enum Species
    {
        Human, 
        Elf, 
        Orc
    }
    public enum Role
    {
        MagicUser,
        Warrior,
        Rogue
    }

    internal class PlayerCharacter
    {

        Texture texture;
        public string Name;
        public Species species;
        public Role role;

        public Vector2 position;

        char image;
        ConsoleColor drawColor;
        int imagePixelX;
        int imagePixelY;


        public void SetImageAndIndex(Texture Image, int imagesPerRow, int Index)
        {
            texture = Image;
            imagePixelX = (Index % imagesPerRow) * Game.tileSize;
            imagePixelY = (int)(Index / imagesPerRow) * Game.tileSize;
        }


        public void Draw()
        {
            //graphicsMode = GameGraphicsMode.Console;


            int imagesPerRow = 2;
            int tileSize = 100;

            // Surullisen kissan koordinaatit
            int X = 0;
            int Y = 1;

            // indeksit ovat:
            // 0, 1
            // 2, 3
            // joten atlasIndex on 2
            int atlasIndex = Y * imagesPerRow + X;

            // Laske kuvan kohta
            int imageX = atlasIndex % imagesPerRow; // 2 % 2 = 0
            int imageY = (int)(atlasIndex / imagesPerRow); // 2 / 2 = 1
            int imagePixelX = imageX * tileSize; // 0 * 100 = 0
            int imagePixelY = imageY * tileSize; // 1 * 100 = 100

            // Laske suorakulmio
            Rectangle imageRect = new Rectangle(imagePixelX, imagePixelY, tileSize, tileSize);

            // Laske paikka ruudulla
            Vector2 position = new Vector2(2, 5);
            int pixelPositionX = (int)(position.X * tileSize);
            int pixelPositionY = (int)(position.Y * tileSize);
            Vector2 pixelPosition = new Vector2(pixelPositionX, pixelPositionY);
        } 


    }
}
