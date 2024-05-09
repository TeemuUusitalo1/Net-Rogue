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
    }
}

