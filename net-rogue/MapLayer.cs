using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net_rogue
{
    internal class MapLayer
    {
        public string name;
        public int[] data;
        public int height;
        public int width;
        public MapLayer(int mapSize)
        {
            height = 20;
            width = 30;
            name = "";
            data = new int[mapSize];
        }

    }
}
