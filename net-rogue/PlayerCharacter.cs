using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Threading;
using ZeroElectric.Vinculum;

namespace net_rogue
{

    public enum Species
    {
        Human, 
        Elf, 
        Orc
    }
    public enum Role
    {
        MagicUser,
        Warrior,
        Rogue
    }

    internal class PlayerCharacter
    {

        Texture texture;
        public string Name;
        public Species species;
        public Role role;

        public Vector2 position;

        char image;
        ConsoleColor drawColor;

        public void SetImageAndIndex(Texture Image)
        {
            texture = Image;

        }




    }
}
