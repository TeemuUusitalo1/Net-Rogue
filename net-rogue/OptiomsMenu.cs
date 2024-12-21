using RayGuiCreator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroElectric.Vinculum;

namespace net_rogue
{
    internal class OptionsMenu
    {
        public EventHandler BackButtonPressedEvent;

        public void DrawMenu()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.DARKGRAY);

            MenuCreator c  = new MenuCreator(10,10, Raylib.GetScreenHeight() / 20, Raylib.GetScreenHeight() / 4, 3, + 2);
            c.Label("Options");

            if (c.Button("Back"))
            {
                BackButtonPressedEvent(this, EventArgs.Empty);
            }

            Raylib.EndDrawing();

        }
    }
}
