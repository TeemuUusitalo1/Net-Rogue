using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Numerics;
using System.IO.MemoryMappedFiles;
using System.IO.Enumeration;
using TurboMapReader;


namespace net_rogue
{
    internal class MapLoader
    {
        private bool fileFound;

        public Map LoadTestMap()
        {
            Map test = new Map();

            test.mapWidth = 8;
            test.data = new int[] {
                2, 2, 2, 2, 2, 2, 2, 2,
                2, 1, 1, 2, 1, 1, 1, 2,
                2, 1, 1, 2, 1, 1, 1, 2,
                2, 1, 1, 1, 1, 1, 2, 2,
                2, 2, 2, 2, 1, 1, 1, 2,
                2, 1, 1, 1, 1, 1, 1, 2,
                2, 2, 2, 2, 2, 2, 2, 2 };
            return test;
        }


        public void TestFileReading(string filename)
        {
            using (StreamReader reader = File.OpenText(filename))
            {
                Console.WriteLine("File contents:");
                Console.WriteLine();

                string line;
                while (true)
                {
                    line = reader.ReadLine();
                    if (line == null)
                    {
                        break; // End of file
                    }
                    Console.WriteLine(line);
                }
            }
        }

        public Map LoadMapFromFile(string filename)
        {
            fileFound = File.Exists(filename);

            if (fileFound == false)
            {
                Console.WriteLine($"File {filename} not found");
                return LoadTestMap(); // Return the test map as fallback
            }

            string fileContents = "";
        
            {



                using (StreamReader reader = File.OpenText(filename))
                {

                    fileContents = reader.ReadToEnd();
                    Console.WriteLine("Contains: " + fileContents);

                }


            }
            Map loadedMap = JsonConvert.DeserializeObject<Map>(fileContents);

            return loadedMap;
        }
        public Map? ReadMapFromFile(string filename)
        {
            // Lataa tiedosto käyttäen TurboMapReaderia   
            TurboMapReader.TiledMap mapMadeInTiled = TurboMapReader.MapReader.LoadMapFromFile(filename);

            // Tarkista onnistuiko lataaminen
            if (mapMadeInTiled != null)
            {
                // Muuta Map olioksi ja palauta
                return ConvertTiledMapToMap(mapMadeInTiled);
            }
            else
            {
                // OH NO!
                return null;
            }
        }

        public Map ConvertTiledMapToMap(TiledMap turboMap)
        {
            // Luo tyhjä kenttä
            Map rogueMap = new Map
            {
                // Asetetaan kentän leveys ja korkeus TurboMapReaderin tiedoista
                mapWidth = turboMap.width,
                mapHeight = turboMap.height
            };

            // Muunna tason "ground" tiedot
            TurboMapReader.MapLayer groundLayer = turboMap.GetLayerByName("ground");
            if (groundLayer != null)
            {
                int howManyTiles = groundLayer.data.Length; // Kuinka monta kenttäpalaa tasossa on
                int[] groundTiles = groundLayer.data;

                // Luo uusi taso ja kopioi tiedot
                MapLayer myGroundLayer = new MapLayer(howManyTiles)
                {
                    name = "ground",
                    data = groundTiles // Kopioidaan kaikki palat
                };

                // Tallenna taso kenttään
                rogueMap.layers[0] = myGroundLayer;
            }

            // Muunna tason "enemies" tiedot
            TurboMapReader.MapLayer enemyLayer = turboMap.GetLayerByName("enemies");
            if (enemyLayer != null)
            {
                int howManyEnemies = enemyLayer.data.Length; // Kuinka monta vihollista tasossa on
                int[] enemyTiles = enemyLayer.data;

                // Luo uusi taso ja kopioi tiedot
                MapLayer myEnemyLayer = new MapLayer(howManyEnemies)
                {
                    name = "enemies",
                    data = enemyTiles // Kopioidaan kaikki viholliset
                };

                // Tallenna taso kenttään
                rogueMap.layers[1] = myEnemyLayer;
            }

            // Muunna tason "items" tiedot
            TurboMapReader.MapLayer itemLayer = turboMap.GetLayerByName("items");
            if (itemLayer != null)
            {
                int howManyItems = itemLayer.data.Length; // Kuinka monta esinettä tasossa on
                int[] itemTiles = itemLayer.data;

                // Luo uusi taso ja kopioi tiedot
                MapLayer myItemLayer = new MapLayer(howManyItems)
                {
                    name = "items",
                    data = itemTiles // Kopioidaan kaikki esineet
                };

                // Tallenna taso kenttään
                rogueMap.layers[2] = myItemLayer;
            }

            // Lopulta palauta kenttä
            return rogueMap;
        }

    }
}

