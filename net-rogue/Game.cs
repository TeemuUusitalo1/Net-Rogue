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

        enum GameState
        {
            MainMenu,
            CharacterCreation,
            GameLoop
        }
        private string AskName()
        {
            string nameAnswer;
            while (true)
            {
                Console.WriteLine("What is your name?");
                nameAnswer = Console.ReadLine();

                if (String.IsNullOrEmpty(nameAnswer))
                {
                    Console.WriteLine("Name cannot be empty!");
                    continue;
                }




                bool nameOk = true; // Expect name to be ok
                for (int i = 0; i < nameAnswer.Length; i++)
                {
                    char letter = nameAnswer[i]; // check all characters
                    if (Char.IsLetter(letter) == false)
                    {
                        // found something that is _not_ a letter
                        nameOk = false;
                        break; // break the for loop
                    }
                }
                if (nameOk == false)
                {
                    Console.WriteLine("Name can only contain letters!");
                    continue; // Ask again
                }
                break;
            }
            return nameAnswer;

        }

        private Species AskSpecies()
        {
            Species choice = Species.Human;
            while (true)
            {
                Console.WriteLine("Select species for your character:");
                Console.WriteLine($"1: {Species.Human.ToString()}");
                Console.WriteLine($"2: {Species.Elf.ToString()}");
                Console.WriteLine($"3: {Species.Orc.ToString()}");
                string answer = Console.ReadLine();

                if (answer == "1")
                {
                    choice = Species.Human;
                }
                else if (answer == "2")
                {
                    choice = Species.Elf;
                }
                else if (answer == "3")
                {
                    choice = Species.Orc;
                }
                else
                {
                    Console.WriteLine("Invalid choice!");
                    continue;
                }
                return choice;
            }
        }

        private Role AskRole()
        {
            Role choice = Role.MagicUser;
            while (true)
            {
                Console.WriteLine("Select a Role for your character:");
                Console.WriteLine($"1: {Role.Rogue.ToString()}");
                Console.WriteLine($"2: {Role.Warrior.ToString()}");
                Console.WriteLine($"3: {Role.MagicUser.ToString()}");
                string answer = Console.ReadLine();

                if (answer == "1")
                {
                    choice = Role.Rogue;
                }
                else if (answer == "2")
                {
                    choice = Role.Warrior;
                }
                else if (answer == "3")
                {
                    choice = Role.MagicUser;
                }
                else
                {
                    Console.WriteLine("Invalid choice!");
                    continue;
                }

                return choice;
            }
        }


        private PlayerCharacter CreateCharacter()
        {
            PlayerCharacter player = new PlayerCharacter();

            player.Name = AskName();
            player.species = AskSpecies();
            player.role = AskRole();
            return player;
        }

        public void Run()
        {


            Console.Clear();
            Init();
            GameLoop();

        }


        private void Init()
        {

            player = CreateCharacter();

            Raylib.InitWindow(800, 600, "Image_test");
            Raylib.SetTargetFPS(30);
            Raylib.SetWindowState(ConfigFlags.FLAG_WINDOW_RESIZABLE);
            // drawMode = GameDrawMode.TopDown2D;
            game_width = 480 / 2;
            game_height = 270 / 2;

            game_screen = Raylib.LoadRenderTexture(game_width, game_height);
            Raylib.SetTextureFilter(game_screen.texture, TextureFilter.TEXTURE_FILTER_POINT);
            Raylib.SetWindowMinSize(game_width, game_height);
            tilesheet = Raylib.LoadTexture("Images/tilemap_packed.png");


            TurboMapReader.TiledMap tiledMap = TurboMapReader.MapReader.LoadMapFromFile("Maps/mapfile.json");

            int mapWidth = tiledMap.width;
            int mapHeight = tiledMap.height;

            TurboMapReader.MapLayer groundLayer = tiledMap.GetLayerByName("Ground");

            if (tiledMap == null)
            {
                // error!
            }

            //TurboMapReader.MapLayer groundLayer = tiledMap.GetLayerByName("Ground");
            //TiledMap loadedTileMap = TurboMapReader.MapReader.LoadMapFromFile(f);

            int howManyTiles = groundLayer.data.Length;
            int[] groundTiles = groundLayer.data;
            MapLayer myGroundLayer = new MapLayer();
            myGroundLayer.mapTiles = new int[howManyTiles];

            MapLoader mapread = new MapLoader();

            //level01 = loader.LoadMapFromFile("mapfile.json");
            //level01 = mapread.LoadLayeredMap("Maps/mapfile_layers.json");
            level = mapread.LoadMapFromFile("Maps/MapsFile.tmj");
            level.SetTileSheet(tilesheet);
            player.SetImageAndIndex(tilesheet, 12, 8 * 12 + 3);
            Texture imageTexture = Raylib.LoadTexture("image.png");

            // Rectange imageRect = new Rectangle(imagePixelX, imagePixelY, Game.tileSize, Game.tileSize);


            currentGameState = GameState.MainMenu;

        }

        //void IsWall(intx inty)

        private Vector2 position;

        private void DrawGame()
        {
            Raylib.BeginDrawing();


            //void DrawTextureV(Texture2D texture, Vector2 position, Color tint);

            Console.Clear();
            level.Draw();
            player.Draw();

            Raylib.EndDrawing();
        }

        private void UpdateGame()
        {



            {

                player.position = new Vector2(1, 1);


                Console.SetCursorPosition((int)player.position.X, (int)player.position.Y);
                Console.Write("@");

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
                
                // TODO: CHECK COLLISION WITH WALLS
                //

                MapTile tile = level.GetTileAt(moveX, moveY);
                if (tile == MapTile.Floor)
                {
                    // Voi liikkua
                }
                else if (tile == MapTile.Wall)
                {
                    // Ei voi liikkua
                }
                //



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
            // Tyhjennä ruutu ja aloita piirtäminen
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.BLACK);


            // Laske ylimmän napin paikka ruudulla.
            int button_width = 100;
            int button_height = 20;
            int button_x = Raylib.GetScreenWidth() / 2 - button_width / 2;
            int button_y = Raylib.GetScreenHeight() / 2 - button_height / 2;

            // Piirrä pelin nimi nappien yläpuolelle
            RayGui.GuiLabel(new Rectangle(button_x, button_y - button_height * 2, button_width, button_height), "Rogue");

            if (RayGui.GuiButton(new Rectangle(button_x, button_y, button_width, button_height), "Start Game") == 1)
            {
                // currentState = GameState.GameLoop;
            }
            button_y += button_height * 2;

            if (RayGui.GuiButton(new Rectangle(button_x,
                button_y,
                button_width, button_height), "Options") == 1)
            {
                // Go to options somehow
            }

            button_y += button_height * 2;

            if (RayGui.GuiButton(new Rectangle(button_x,
                button_y,
                button_width, button_height), "Quit") == 1)
            {
                // Quit the game
            }
            Raylib.EndDrawing();

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
                        Raylib.ClearBackground(Raylib.DARKGRAY);
                        DrawMainMenu();
                        Raylib.EndDrawing();
                        break;

                    case GameState.GameLoop:
                        UpdateGame();
                        DrawGame();
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



