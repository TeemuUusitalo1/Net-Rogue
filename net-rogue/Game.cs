using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net_rogue
{
    internal class Game
    {
        public void Run()
        {
            PlayerCharacter player = new PlayerCharacter();

            
            Console.WriteLine("What is your name?");
            string nimi = Console.ReadLine();

            //player name = Console.ReadLine();
            
            Console.WriteLine("Select Race");
            Console.WriteLine("1: Human");
            Console.WriteLine("2: Elf");
            Console.WriteLine("3: Orc");
            string raceAnswer = Console.ReadLine();

            Console.WriteLine("Valitse luokka;");
            Console.WriteLine(Role.Rogue.ToString());
            Console.WriteLine(Role.Warrior.ToString());
            Console.WriteLine(Role.MagicUser.ToString());

            if (raceAnswer == Species.Human.ToString() )
            {
                player.species = Species.Human;
            }
        }
    }
}
