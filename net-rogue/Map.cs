using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroElectric.Vinculum;

namespace net_rogue
{
    internal class Map
    {
        public int mapWidth;
        public int[] mapTiles;
        Texture texture;
        

        public void SetTileSheet(Texture Image)
        {
            texture = Image;
        }

        public int GetTileAt(int x, int y)
        {
            if (x < 0 || y < 0)
            {
                return x;
            }
            return 0;
        }
        public void Draw()
        {


            Console.ForegroundColor = ConsoleColor.Gray; // Change to map color
            int mapHeight = mapTiles.Length / mapWidth; // Calculate the height: the amount of rows

            for (int y = 0; y < mapHeight; y++) // for each row
            {
                for (int x = 0; x < mapWidth; x++) // for each column in the row
                {
                    int index = x + y * mapWidth; // Calculate index of tile at (x, y)
                    int tileId = mapTiles[index]; // Read the tile value at index

                    // Draw the tile graphics
                    Console.SetCursorPosition(x, y);
                    switch (tileId)
                    {
                        case 1:
                            Console.Write("."); // Floor
                            break;
                        case 2:
                            Console.Write("#"); // Wall
                            break;
                        default:
                            Console.Write(" ");
                            break;
                    }
                }
            }

        }
    }
}
