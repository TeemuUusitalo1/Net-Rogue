﻿using System;
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

        Texture myImage;

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

            MapLoader mapread = new MapLoader();

            //MapLoader loader = new MapLoader();
            //level01 = loader.LoadMapFromFile("mapfile.json");
            //level01 = mapread.LoadLayeredMap("Maps/mapfile_layers.json");
            level = mapread.LoadMapFromFile("Maps/mapfile.json");
            level.SetTileSheet(tilesheet);
            player.SetImageAndIndex(tilesheet, 8 * 12 + 3);
            Texture imageTexture = Raylib.LoadTexture("image.png");

            //graphicsMode = GameGraphicsMode.Console;

            myImage = imageTexture;
        }



        private Vector2 position;



        private void DrawGame()
        {
            Raylib.BeginDrawing();

            Raylib.DrawTextureV(myImage, position, Raylib.WHITE);

            //void DrawTextureV(Texture2D texture, Vector2 position, Color tint);

            Console.Clear();
            level01.Draw();
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



            

                //
                // TODO: CHECK COLLISION WITH WALLS
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

        void GameLoop()
        {
            while (Raylib.WindowShouldClose() == false)
            {
                DrawGame();
                UpdateGame();
   
            }
            Raylib.UnloadTexture(imageTexture);

            Raylib.CloseWindow();

        }
    }
}
