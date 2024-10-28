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
        Texture myImage;


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
            Raylib.DrawTextureV(myImage, position, Raylib.WHITE);


            int imagesPerRow = 2;
            int tileSize = 100;


        } 


    }
}
