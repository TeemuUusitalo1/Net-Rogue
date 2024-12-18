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


        public string Name;
        public Species species;
        public Role role;
        Texture myImage;


        public Vector2 position;

        char image;
        ConsoleColor drawColor;
        int imagePixelX;
        int imagePixelY;
        Rectangle imageRect;

        public void SetImageAndIndex(Texture Image, int imagesPerRow, int Index)
        {
            myImage = Image;
            imagePixelX = (Index % imagesPerRow) * Game.tileSize;
            imagePixelY = (int)(Index / imagesPerRow) * Game.tileSize;

           imageRect = new Rectangle(imagePixelX, imagePixelY, Game.tileSize, Game.tileSize);
        }


        public void Draw()
        {

            //graphicsMode = GameGraphicsMode.Console;
            Vector2 pixelPos = new Vector2(position.X * Game.tileSize, position.Y * Game.tileSize);
            Raylib.DrawTextureRec(myImage, imageRect, pixelPos, Raylib.WHITE);

            int imagesPerRow = 2;
            int tileSize = 100;


        } 


    }
}
