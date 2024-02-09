using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

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
        public string Name;
        public Species species;
        public Role role;

        public Vector2 position;
    }
}
