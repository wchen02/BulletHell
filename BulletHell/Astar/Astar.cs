using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace BulletHell
{
    public class Astar : GameState
    {
        public Texture2D background;
        public MoveableObj hero;
        public MoveableObj enemy;
        public Texture2D white;
        public bool[,] world;
        public int distance, cost, totalcost;

        public Astar(GameStateManager gameStateManager)
            : base(gameStateManager)
        {
            isActive = true;
            needUpdate = true;
            Initialize();
        }

        public void Initialize()
        {
            LoadContent();
            initialized = true;
        }

        public void LoadContent() {
            spriteBatch = gameStateManager.spriteBatch;
            Content = gameStateManager.content;

            hero = new MoveableObj(Content.Load<Texture2D>("Astar//sprites//character"), new Vector2(0, 50), new Vector2(2, 2));
            enemy = new MoveableObj(Content.Load<Texture2D>("Astar//sprites//alien"), new Vector2(80, 50), Vector2.Zero);
            white = new Texture2D(Content.Load<Texture2D>("Astar//sprites//whitedot"), 1, 1);
            background = Content.Load<Texture2D>("Astar//sprites//background");
            world = new bool[10,10]{}; // default to false
        }

        public void UnloadContent();

        public void Update(GameTime gameTime) { 
        
        }

        public void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            for (int i = 0; i < 10; ++i)
            {
                for (int j = 0; j < 10; ++j){
                    if (world[i][j] == true)
                        spriteBatch.Draw(white, new Rectangle(i * 10, i * 10, 10, 10), Color.White);
      
               }
            }
            spriteBatch.End();
        }

        public void Astar(int start, int goal);

        /* f(n) = g(n) + h(n)
         * g(n) represents the cost of the path from the starting point to any vertex n, 
         * and h(n) represents the heuristic estimated cost from vertex n to the goal. 
         */

        //public int g(n);
        //public int f(n);
        //public int h(n);
    }
}
