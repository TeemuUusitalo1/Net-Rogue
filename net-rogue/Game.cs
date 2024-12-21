using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ZeroElectric.Vinculum;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Input;
using TurboMapReader;
using RayGuiCreator;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections;

namespace net_rogue
{
    internal class Game
    {

        PlayerCharacter player = new PlayerCharacter();
        Map level;
        Texture tilesheet;
        public int game_width = 800;
        public int game_height = 800;
        public RenderTexture game_screen;
        public static int tileSize = 16;
        GameState currentGameState;

        OptionsMenu myOptionsMenu;
        PauseMenu myPauseMenu;

        // List of possible difficulty choices. The indexing starts at 0
        MultipleChoiceEntry SpeciesChoices = new MultipleChoiceEntry(
            new string[] { "Human", "Elf", "Orc" });
        // List of possible class choices.
        MultipleChoiceEntry RoleChoices = new MultipleChoiceEntry(
            new string[] { "Rogue", "Warrior", "Magic User" });

        // Textbox data for player's name
        TextBoxEntry playerNameEntry = new TextBoxEntry(15);

        Stack<GameState> stateStack = new Stack<GameState>();

        enum GameState
        {
            MainMenu,
            CharacterCreation,
            GameLoop,
            PauseMenu,
            OptionsMenu
        }
        

        public void Run()
        {


            Console.Clear();
            Init();
            GameLoop();

        }


        private void Init()
        {

            Raylib.InitWindow(800, 600, "Rogue");
            Raylib.SetTargetFPS(30);
            Raylib.SetWindowState(ConfigFlags.FLAG_WINDOW_RESIZABLE);
            
            game_width = 480 / 2;
            game_height = 270 / 2;

            game_screen = Raylib.LoadRenderTexture(game_width, game_height);
            Raylib.SetTextureFilter(game_screen.texture, TextureFilter.TEXTURE_FILTER_POINT);
            Raylib.SetWindowMinSize(game_width, game_height);
            tilesheet = Raylib.LoadTexture("Images/tilemap_packed.png");
            RayGui.GuiLoadStyle("Style.rgs");

            myOptionsMenu = new OptionsMenu();
            myPauseMenu = new PauseMenu();

            myOptionsMenu = new OptionsMenu();
            // Kytke asetusvalikon tapahtumaan funktio
            myOptionsMenu.BackButtonPressedEvent += this.OnOptionsBackButtonPressed;

            myPauseMenu = new PauseMenu();
            // Kytke asetusvalikon tapahtumaan funktio
            myPauseMenu.BackButtonPressedEvent += this.OnPauseBackButtonPressed;
            myPauseMenu.OptionsButtonPressedEvent += this.OnPauseOptionsButtonPressed;
            myPauseMenu.MainMenuButtonPressedEvent += this.OnPauseMainMenuButtonPressed;

            player.position = new Vector2(2, 3);
            TurboMapReader.TiledMap tiledMap = MapReader.LoadMapFromFile("Maps/tiledmap.tmj");

            int mapWidth = tiledMap.width;
            int mapHeight = tiledMap.height;

            TurboMapReader.MapLayer groundLayer = tiledMap.GetLayerByName("ground");

            if (tiledMap == null)
            {
                // error!
            }

            TiledMap loadedTileMap = TurboMapReader.MapReader.LoadMapFromFile("Images/tilemap_packed.png");


            MapLoader mapread = new MapLoader();

            //level01 = loader.LoadMapFromFile("mapfile.json");
            //level01 = mapread.LoadLayeredMap("Maps/mapfile_layers.json");
            level = mapread.LoadMapFromFile("Maps/tiledmap.tmj");

            level.SetTileSheet(tilesheet);
            player.SetImageAndIndex(tilesheet, 12, 8 * 12 + 3);
            level.LoadEnemies();
            level.LoadItems();
            Texture imageTexture = Raylib.LoadTexture("image.png");

            currentGameState = GameState.MainMenu;

        }
        // MENU STUFF ********************************************************************************
        void OnOptionsBackButtonPressed(object sender, EventArgs args)
        {
            if (stateStack.Contains(GameState.PauseMenu) == true)
            {
                stateStack.Pop();
                currentGameState = GameState.PauseMenu;
            }
            else if (stateStack.Contains(GameState.MainMenu) == false)
            {

                currentGameState = GameState.MainMenu;
            }

        }

        void OnPauseBackButtonPressed(object sender, EventArgs args)
        {
            currentGameState = GameState.GameLoop;
            stateStack.Pop();
        }
        void OnPauseOptionsButtonPressed(object sender, EventArgs args)
        {
            On_OptionsMenu_PauseButtonPressed();
        }
        void OnPauseMainMenuButtonPressed(object sender, EventArgs args)
        {
            On_PauseMenu_MainMenuButtonPressed();

        }



        public void On_OptionsMenu_PauseButtonPressed()
        {
            currentGameState = GameState.OptionsMenu;
            stateStack.Push(GameState.OptionsMenu);
        }
        public void On_GameLoop_PauseMenuButtonPressed()
        {
            currentGameState = GameState.PauseMenu;
            stateStack.Push(GameState.PauseMenu);
        }
        public void On_PauseMenu_MainMenuButtonPressed()
        {
            
            stateStack.Clear();
            player.position = new Vector2(2, 3);
            currentGameState = GameState.MainMenu;
        } 


        // MENU STUFF END ****************************************************************************

        private void DrawGame()
        {
            Raylib.BeginDrawing();



            Raylib.ClearBackground(Raylib.GRAY);
            level.Draw();
            player.Draw();

            Raylib.EndDrawing();
        }

        private void UpdateGame()
        {

            {
                Console.SetCursorPosition((int)player.position.X, (int)player.position.Y);


                int moveX = 0;
                int moveY = 0;

                if (Raylib.IsKeyPressed(KeyboardKey.KEY_UP))
                {
                    // Move player up
                    moveY = -1;
                }
                else if (Raylib.IsKeyPressed(KeyboardKey.KEY_DOWN))
                {
                    moveY = 1;
                }
                else if (Raylib.IsKeyPressed(KeyboardKey.KEY_LEFT))
                {
                    moveX = -1;
                }
                else if (Raylib.IsKeyPressed(KeyboardKey.KEY_RIGHT))
                {
                    moveX = 1;
                }
                

                MapTile tile = level.GetTileAt((int) player.position.X + moveX,(int) player.position.Y + moveY);
                if (tile == MapTile.Floor)
                {
                    // Voi liikkua
                    Enemy enemy = level.GetEnemyAt((int)player.position.X + moveX, (int)player.position.Y + moveY);
                    if (enemy == null)
                    {
                        
                    }
                    else
                    {
                        moveX = 0;
                        moveY = 0;
                    }
                    Item item = level.GetItemAt((int)player.position.X + moveX, (int)player.position.Y + moveY);
                    if (item == null)
                    {

                    }
                    else
                    {
                        moveX = 0;
                        moveY = 0;
                    }
                }
                else if (tile == MapTile.Wall)
                {
                    // Ei voi liikkua
                    moveX = 0;
                    moveY = 0;
                }


                if (Raylib.IsKeyPressed(KeyboardKey.KEY_P))
                {

                    On_GameLoop_PauseMenuButtonPressed();

                }

                // Prevent player from going outside screen
                if (player.position.X < 0)
                {
                    player.position.X = 0;
                }
                else if (player.position.X > Console.WindowWidth - 1)
                {
                    player.position.X = Console.WindowWidth - 1;
                }
                if (player.position.Y < 0)
                {
                    player.position.Y = 0;
                }
                else if (player.position.Y > Console.WindowHeight - 1)
                {
                    player.position.Y = Console.WindowHeight - 1;
                }


                // Move the player
                player.position.X += moveX;
                player.position.Y += moveY;

            }


        }

        public void DrawMainMenu()
        {
            // Piirtämisen aloitus
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.BLACK);

            // Määritellään valikon sijainti ja mitat
            int menuStartX = 10;
            int menuStartY = 50;
            int rowHeight = Raylib.GetScreenHeight() / 15;
            int menuWidth = Raylib.GetScreenWidth() / 3;

            // MenuCreator luodaan
            MenuCreator creator = new MenuCreator(menuStartX, menuStartY, rowHeight, menuWidth);

            // Piirretään pelin nimi
            creator.Label("Rogue adventure");

            // Piirretään ohjeteksti
            creator.Label("Ohjeet:\n- Käytä nuolinäppäimiä liikkumiseen\n- Space ampuu\n- Esc sulkee pelin");

            // Piirretään nappi pelin käynnistämiseen
            if (creator.Button("Aloita peli"))
            {
                // Vaihdetaan pelitila pelilooppiin
                currentGameState = GameState.CharacterCreation;
            }

            if (creator.Button("Options"))
            {
                On_OptionsMenu_PauseButtonPressed();
            }

            // Piirretään nappi ohjelman sulkemiseen
            if (creator.Button("Lopeta peli"))
            {
                CleanupAndExit();
            }
            
        }
        private void CleanupAndExit()
        {
            // Unload resources
            Raylib.UnloadTexture(tilesheet);
            Raylib.UnloadRenderTexture(game_screen);

            // Close the Raylib window
            Raylib.CloseWindow();

            // Exit the application
            Environment.Exit(0);
        }

        void DrawCharacterCreationMenu()
        {

            
            int width = Raylib.GetScreenWidth() / 2;
            // Fit 22 rows on the screen
            int rows = 22;
            int rowHeight = Raylib.GetScreenHeight() / rows;
            // Center the menu horizontally
            int x = (Raylib.GetScreenWidth() / 2) - (width / 2);
            // Center the menu vertically
            int y = (Raylib.GetScreenHeight() - (rowHeight * rows)) / 2;
            // 3 pixels between rows, text 3 pixels smaller than row height
            MenuCreator creator = new MenuCreator(x, y, rowHeight, width, 3, -3);
            creator.Label("Main menu");

            creator.Label("Player name");
            creator.TextBox(playerNameEntry);

            creator.Label("Character speices");
            creator.DropDown(SpeciesChoices);

            creator.Label("Character class");
            creator.DropDown(RoleChoices);
            string playerName = playerNameEntry.ToString();
            if (creator.Button("Aloita peli"))
            {
                if (string.IsNullOrEmpty(playerName))
                {
                    Console.WriteLine("Nimi ei saa olla tyhjä!");
                }
                else if (SpeciesChoices == null || RoleChoices == null)
                {
                    Console.WriteLine("Valitse laji ja rooli hahmolle!");
                }
                else
                {

                    Console.WriteLine("Hahmon luonti onnistui!");
                    Console.WriteLine($"Nimi: {playerNameEntry}, Laji: {SpeciesChoices}, Rooli: {RoleChoices}");

                    currentGameState = GameState.GameLoop; // Vaihda pelitilaan
                }
               
            }

            // Draws open dropdowns over other menu items
            int menuHeight = creator.EndMenu();

            // Draws a rectangle around the menu
            int padding = 2;
            Raylib.DrawRectangleLines(
                x - padding,
                y - padding,
                width + padding * 2,
                menuHeight + padding * 2,
                MenuCreator.GetLineColor());

        }




        public void GameLoop()
        {

            while (Raylib.WindowShouldClose() == false)
            {
                switch (currentGameState)
                {
                    case GameState.MainMenu:
                        Raylib.BeginDrawing();
                        Raylib.ClearBackground(Raylib.DARKGRAY);
                        DrawMainMenu();
                        Raylib.EndDrawing();
                        break;

                    case GameState.CharacterCreation:
                        Raylib.BeginDrawing();
                        Raylib.ClearBackground(Raylib.BLACK);
                        DrawCharacterCreationMenu();
                       // DrawMainMenu();
                        Raylib.EndDrawing();
                        break;

                    case GameState.GameLoop:
                        UpdateGame();
                        DrawGame();
                        break;

                    case GameState.OptionsMenu:
                        myOptionsMenu.DrawMenu();
                        break;
                    
                    case GameState.PauseMenu:
                        myPauseMenu.DrawMenu();
                        break;
                }

            }




            Raylib.UnloadTexture(tilesheet);
            Raylib.EndDrawing();
            Raylib.CloseWindow();
        }

        //void OnstateChangeRequested(object sender, GameState newState)
    }
}

    //internal class OptionsMenu
    //{
      //  public event EventHandler<Game.Gamestate> stateChangeRequested;
    //}



