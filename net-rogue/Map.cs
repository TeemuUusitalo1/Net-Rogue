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
        public int mapHeight;
        public MapLayer[] layers;
        public int[] data;
        public static List<int> WallTileNumbers;
        public static List<int> FloorTileNumbers;
        public List<Item> items;
        public List<Enemy> enemies;

        Texture texture;

        private void Init()
        {
            WallTileNumbers = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 12, 13, 14, 15, 16, 17, 18, 19, 20, 24, 25, 26, 27, 28, 29, 40, 57, 58, 59 };
            FloorTileNumbers = new List<int> { };

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
            enemies = new List<Enemy>() { };
            items = new List<Item>() { };
            Init();
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


            // Use the index to get a map tile from map's array
            MapLayer groundLayer = GetLayer("ground");
            int[] data = groundLayer.data;
            int indexInMap = x + y * groundLayer.width;
            int tileId = data[indexInMap];

            if (WallTileNumbers.Contains(tileId))
            {
                // Is a wall
                return MapTile.Wall;
            }
            else 
            {
                // One of the floortiles
                return MapTile.Floor;
            }
        }

        public Enemy GetEnemyAt(int x, int y)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                Enemy currentEnemy = enemies[i];
                Vector2 enemyPosition = currentEnemy.position;
                int enemySpriteIndex = currentEnemy.spriteIndex;

                if (enemyPosition.X == x && enemyPosition.Y == y)
                {
                    return currentEnemy;
                }
     
            }
            return null;
        }

        public Item GetItemAt(int x, int y)
        {
            for (int i = 0; i < items.Count; i++)
            {
                Item currentItem = items[i];
                Vector2 itemPosition = currentItem.position;
                int itemSpriteIndex = currentItem.spriteIndex;

                if (itemPosition.X == x && itemPosition.Y == y)
                {
                    return currentItem;
                }

            }
            return null;
        }

        public string GetEnemyName(int spriteIndex)
        {
            switch (spriteIndex)
            {
                case 108: return "Ghost"; break;
                case 109: return "Cyclops"; break;
                default: return "Unknown"; break;
            }
        }
        public string GetItemName(int spriteIndex)
        {
            switch (spriteIndex)
            {
                case 108: return "Potion"; break;
                case 109: return "Sword"; break;
                default: return "Unknown"; break;
            }
        }

        public void LoadEnemies()
        {
            // Hae viholliset sisältävä taso kentästä
            MapLayer enemyLayer = GetLayer("enemies");
            int[] enemyTiles = enemyLayer.data;
            int layerHeight = enemyTiles.Length / enemyLayer.width;

            // Käy taso läpi ja luo viholliset
            for (int y = 0; y < layerHeight; y++)
            {
                for (int x = 0; x < enemyLayer.width; x++)
                {
                    // Laske paikka valmiiksi
                    Vector2 position = new Vector2(x, y);

                    int index = x + y * enemyLayer.width;

                    int tileId = enemyTiles[index];

                    if (tileId == 0)
                    {
                        // Tässä kohdassa kenttää ei ole vihollista
                        continue;
                    }
                    else
                    {
                        // Tässä kohdassa kenttää on jokin vihollinen

                        // Tässä pitää vähentää 1,
                        // koska Tiled editori tallentaa
                        // palojen numerot alkaen 1:sestä.
                        int spriteId = tileId - 1;

                        // Hae vihollisen nimi
                        string name = GetEnemyName(spriteId);

                        // Luo uusi vihollinen ja lisää se listaan
                        enemies.Add(new Enemy(name, position, spriteId));
                    }
                }
            }
        }
        public void LoadItems()
        {
            // Hae viholliset sisältävä taso kentästä
            MapLayer ItemLayer = GetLayer("items");
            int[] itemTiles = ItemLayer.data;
            int layerHeight = itemTiles.Length / ItemLayer.width;

            // Käy taso läpi ja luo viholliset
            for (int y = 0; y < layerHeight; y++)
            {
                for (int x = 0; x < ItemLayer.width; x++)
                {
                    // Laske paikka valmiiksi
                    Vector2 position = new Vector2(x, y);

                    int index = x + y * ItemLayer.width;

                    int tileId = itemTiles[index];

                    if (tileId == 0)
                    {
                        // Tässä kohdassa kenttää ei ole vihollista
                        continue;
                    }
                    else
                    {
                        // Tässä kohdassa kenttää on jokin vihollinen

                        // Tässä pitää vähentää 1,
                        // koska Tiled editori tallentaa
                        // palojen numerot alkaen 1:sestä.
                        int spriteId = tileId - 1;

                        // Hae vihollisen nimi
                        string name = GetItemName(spriteId);

                        // Luo uusi vihollinen ja lisää se listaan
                        items.Add(new Item(name, position, spriteId));
                    }
                }
            }
        }
        public void Draw()
        {
            Console.ForegroundColor = ConsoleColor.Gray; // Muutetaan kartan väri
            MapLayer groundLayer = GetLayer("ground");
            int[] data = groundLayer.data;
            int mapHeight = data.Length / groundLayer.width; // Lasketaan korkeuden rivien määrä

            // Spriten leveys ja korkeus (oletetaan, että kaikki palat ovat saman kokoisia)
            int spriteWidth = Game.tileSize;
            int spriteHeight = Game.tileSize;

            // Lasketaan montako spritea on yhden rivin sisällä sprite-arkissa
            int tilesPerRow = texture.width / spriteWidth;

            for (int y = 0; y < mapHeight; y++) // Käydään läpi jokainen rivi
            {
                for (int x = 0; x < groundLayer.width; x++) // Jokainen sarake rivillä
                {
                    int index = x + y * groundLayer.width; // Lasketaan ruudun indeksi
                    int tileId = data[index]; // Haetaan ruudun arvo indeksistä

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

            for (int i = 0; i < enemies.Count; i++)
            {
                Enemy currentEnemy = enemies[i];
                Vector2 enemyPosition = currentEnemy.position;
                int enemySpriteIndex = currentEnemy.spriteIndex;

                int sourceX = (enemySpriteIndex % tilesPerRow) * spriteWidth;
                int sourceY = (enemySpriteIndex / tilesPerRow) * spriteHeight;

                Rectangle sourceRect = new Rectangle(sourceX, sourceY, spriteWidth, spriteHeight);
                Vector2 pixelPos = new Vector2(enemyPosition.X * Game.tileSize, enemyPosition.Y * Game.tileSize);
                Raylib.DrawTextureRec(texture, sourceRect, pixelPos, Raylib.WHITE);
            }

            for (int i = 0; i < items.Count; i++)
            {
                Item currentItem = items[i];
                Vector2 itemPosition = currentItem.position;
                int itemSpriteIndex = currentItem.spriteIndex;

                int sourceX = (itemSpriteIndex % tilesPerRow) * spriteWidth;
                int sourceY = (itemSpriteIndex / tilesPerRow) * spriteHeight;

                Rectangle sourceRect = new Rectangle(sourceX, sourceY, spriteWidth, spriteHeight);
                Vector2 pixelPos = new Vector2(itemPosition.X * Game.tileSize, itemPosition.Y * Game.tileSize);
                Raylib.DrawTextureRec(texture, sourceRect, pixelPos, Raylib.WHITE);
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
