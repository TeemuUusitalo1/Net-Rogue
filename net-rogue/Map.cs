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
        public MapLayer[] layers;
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
                    Raylib.DrawText(tileId.ToString(), x, y,Game.tileSize , Raylib.WHITE);

                }
            }

        }

        public MapLayer GetLayer(string layerName)
        {
            for (int i = 0; i < layers.Length; i++)
            {
                if (layers[i].name == layerName)
                {
                    return layers[i];
                }
            }
            Console.WriteLine($"Error: No layer with name: {layerName}");
            return null; // Wanted layer was not found!
        }

    }

    class MapLayer
    {
        public string name;
        public int[] mapTiles;
    }

}
