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
            test.mapTiles = new int[] {
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

            if (fileFound == false)
            {
                Console.WriteLine($"File {filename} not found");
                return LoadTestMap(); // Return the test map as fallback
            }

            string fileContents = "";
        
            {



                using (StreamReader reader = File.OpenText("mapfile.json"))
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
            // Create an empty map
            Map rogueMap = new Map();

            // Convert the "ground" layer data
            TurboMapReader.MapLayer groundLayer = turboMap.GetLayerByName("ground");

            // Read the map width. All TurboMapReader.MapLayer objects have the same width
            int mapWidth = groundLayer.width;

            // How many tiles are in this layer?
            int howManyTiles = groundLayer.data.Length;
            // Array where the tiles are
            int[] groundTiles = groundLayer.data;

            // Create a new layer based on the data
            MapLayer myGroundLayer = new MapLayer(howManyTiles);
            myGroundLayer.name = "ground";

            // Read the layer tiles
            for (int i = 0; i < howManyTiles; i++)
            {
                myGroundLayer.mapTiles[i] = groundTiles[i];
            }

            // Save the layer to the map
            rogueMap.layers[0] = myGroundLayer;

            // Convert the "enemies" layer data...
            TurboMapReader.MapLayer enemiesLayer = turboMap.GetLayerByName("enemies");
            // TODO: Implement enemy conversion logic

            // Convert the "items" layer data...
            TurboMapReader.MapLayer itemsLayer = turboMap.GetLayerByName("items");
            // TODO: Implement item conversion logic

            // Finally, return the map
            return rogueMap;
        }
    }
}

