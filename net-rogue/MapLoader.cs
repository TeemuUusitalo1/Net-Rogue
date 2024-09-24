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

        public Map ConvertTiledMapToMap(TiledMap turboMap)
        {
            // Luo tyhjä kenttä
            Map rogueMap = new Map();

            // Muunna tason "ground" tiedot
            TurboMapReader.MapLayer groundLayer = turboMap.GetLayerByName("ground");

            // TODO: Lue kentän leveys. Kaikilla TurboMapReader.MapLayer olioilla on sama leveys

            // Kuinka monta kenttäpalaa tässä tasossa on?
            int howManyTiles = groundLayer.data.Length;
            // Taulukko jossa palat ovat
            int[] groundTiles = groundLayer.data;

            // Luo uusi taso tietojen perusteella
            MapLayer myGroundLayer = new MapLayer(howManyTiles);
            myGroundLayer.name = "ground";


            // TODO: lue tason palat



            // Tallenna taso kenttään
            rogueMap.layers[0] = myGroundLayer;

            // TODO: Muunna tason "enemies" tiedot...
            // TODO: Muunna tason "items" tiedot...

            // Lopulta palauta kenttä
            return rogueMap;
        }
    }
}

