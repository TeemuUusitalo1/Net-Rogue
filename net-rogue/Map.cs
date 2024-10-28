using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ZeroElectric.Vinculum;

namespace net_rogue
{
    public enum MapTile : int
    {
        Floor = 48,
        Wall = 40
    }

    internal class Map
    {
        public int mapWidth;
        private int mapHeight;
        public MapLayer[] layers;
        public int[] mapTiles;

        Texture texture;

        
        public void SetTileSheet(Texture Image)
        {
            texture = Image;
        }
        public MapTile GetTileAt(int x, int y)
        {
            // Calculate index: index = x + y * mapWidth
            int indexInMap = x + y * mapWidth;
            // Use the index to get a map tile from map's array
            int mapTileAtIndex = mapTiles[indexInMap];
            return (MapTile)mapTileAtIndex;
        }

        public Map()
        {
            mapWidth = 1;
            mapHeight = 1;
            layers = new MapLayer[3];
            for (int i = 0; i < layers.Length; i++)
            {
                layers[i] = new MapLayer(mapWidth * mapHeight);
            }
           // enemies = new List<Enemy>() { };
           // items = new List<Item>() { };
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

                    // NOTE: tähän kohtaan palataan tehtävässä T06
                    int spriteId = tileId;

                    // TODO:
                    // Käytä samaa koodia kaikkien palojen piirtämiseen:
                    // Tarvitset muuttujat: spriteId, x, y, tilesPerRow

                    // TODO:
                    // Laske palan pikselikordinaatit kuvassa spriteIndex;in avulla
                    // Laske palalle suorakulmio (Rectangle)
                    // Laske pikselikordinaatit, joihin pala piirretään
                    // Käytä piirtämiseen Raylib.DrawTextureRec()

                }
            }

        }
        public Vector2 GetSpritePosition(int spriteIndex, int spritesPerRow)
        {
            float spritePixelX = (spriteIndex % spritesPerRow) * Game.tileSize;
            float spritePixelY = (int)(spriteIndex / spritesPerRow) * Game.tileSize;
            return new Vector2(spritePixelX, spritePixelY);
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
    
}
