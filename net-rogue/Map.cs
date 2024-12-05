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
        public static List<int> WallTileNumbers;
        public static List<int> FloorTileNumbers;


        Texture texture;

        private static void Init()
        {
            WallTileNumbers = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 12, 13, 14, 15, 16, 17, 18, 19, 20, 24, 25, 26, 27, 28, 29, 40, 57, 58, 59 };
            FloorTileNumbers = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 12, 13, 14, 15, 16, 17, 18, 19, 20, 24, 25, 26, 27, 28, 29, 40, 57, 58, 59 };

        }
        public void SetTileSheet(Texture Image)
        {
            texture = Image;
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
            //items = new List<Item>() { };
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

        public MapTile GetTileAt(int x, int y)
        {
            // Calculate index: index = x + y * mapWidth
            int indexInMap = x + y * mapWidth;

            // Use the index to get a map tile from map's array
            MapLayer groundLayer = GetLayer("ground");
            int[] mapTiles = groundLayer.mapTiles;
            int tileId = mapTiles[indexInMap];

            if (WallTileNumbers.Contains(tileId))
            {
                // Is a wall
                return MapTile.Wall;
            }
            else if (FloorTileNumbers.Contains(tileId))
            {
                // One of the floortiles
                return MapTile.Floor;
            }
            else
            {
                // Count everything else as wall for now.
                return MapTile.Wall;
            }
        }



        public void Draw()
        {


            // NOTE: tähän kohtaan palataan tehtävässä T06
            //int spriteId = tileId - 1;

            // TODO:
            // Käytä samaa koodia kaikkien palojen piirtämiseen:
            // Tarvitset muuttujat: spriteId, x, y, tilesPerRow

            // TODO:
            // Laske palan pikselikordinaatit kuvassa spriteIndex;in avulla
            // Laske palalle suorakulmio (Rectangle)
            // Laske pikselikordinaatit, joihin pala piirretään
            // Käytä piirtämiseen Raylib.DrawTextureRec()
            Console.ForegroundColor = ConsoleColor.Gray; // Muutetaan kartan väri
            MapLayer groundLayer = GetLayer("ground");
            int[] mapTiles = groundLayer.mapTiles;
            int mapHeight = mapTiles.Length / mapWidth; // Lasketaan korkeuden rivien määrä

            // Spriten leveys ja korkeus (oletetaan, että kaikki palat ovat saman kokoisia)
            int spriteWidth = Game.tileSize;
            int spriteHeight = Game.tileSize;

            // Lasketaan montako spritea on yhden rivin sisällä sprite-arkissa
            int tilesPerRow = texture.width / spriteWidth;

            for (int y = 0; y < mapHeight; y++) // Käydään läpi jokainen rivi
            {
                for (int x = 0; x < mapWidth; x++) // Jokainen sarake rivillä
                {
                    int index = x + y * mapWidth; // Lasketaan ruudun indeksi
                    int tileId = mapTiles[index]; // Haetaan ruudun arvo indeksistä

                    // Huomioidaan, että tileId vastaa spriteIndexiä sprite-arkissa
                    int spriteId = tileId - 1; // Oletetaan tileId alkaa 1:stä ja spriteId 0:sta

                     // Lasketaan sprite-arkista spriteId:n pikselikoordinaatit
                    int sourceX = (spriteId % tilesPerRow) * spriteWidth; // X-koordinaatti
                    int sourceY = (spriteId / tilesPerRow) * spriteHeight; // Y-koordinaatti

                    // Suorakulmio sprite-arkin koordinaateille
                    Rectangle sourceRect = new Rectangle(sourceX, sourceY, spriteWidth, spriteHeight);

                    // Kohteeseen piirtämisen koordinaatit (ruudulle)
                    Vector2 targetPosition = new Vector2(x * spriteWidth, y * spriteHeight);

                    // Käytetään Raylib.DrawTextureRec tekstuurin piirtämiseen
                    Raylib.DrawTextureRec(texture, sourceRect, targetPosition, Raylib.WHITE);
                }
            }
        }
        public Vector2 GetSpritePosition(int spriteIndex, int spritesPerRow)
        {
            float spritePixelX = (spriteIndex % spritesPerRow) * Game.tileSize;
            float spritePixelY = (int)(spriteIndex / spritesPerRow) * Game.tileSize;
            return new Vector2(spritePixelX, spritePixelY);
        }




    }
    
}
