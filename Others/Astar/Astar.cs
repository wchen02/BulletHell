using System;
using System.Collections.Generic;
using System.Collections;
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

/*
 *  A* Algorithm demostration
 * How to play: 
 * Press Enter to restart.
 * Press Up and Down to configure ratio of blocks
 * Press P to pause
 * Press Esc to return to the main menu
 * By Wensheng Chen
 * This code can be separated from my game state manager
 * simply by putting commenting out some of the statements
 * such as graphics, spriteBatch, and content managee
 */

namespace Astar
{
    public class Tile
    {
        #region Tile
        public Vector2 pos;
        public bool blocked;
        public int rank;
        public Tile pre;

        public Tile(Vector2 position, bool isBlocked)
        {
            pos = position;
            blocked = isBlocked;
            pre = null;
            rank = 0;
        }
        #endregion
    }

    public class Astar : BulletHell.GameState
    {
        #region Fields
        //GraphicsDeviceManager graphics;
        //SpriteBatch spriteBatch;
        SpriteFont font;
        List<Tile> open, close;
        Tile[,] world;
        int x, y;
        float rateOfBlocks = 0.3f;
        Texture2D tile;
        Tile startTile, goalTile, walker;
        Random rand = new Random();
        KeyboardState keyboardState, preKeyboardState;
        Vector2 stepPos, robPos, fontPos, restartPos;
        List<Vector2> path = new List<Vector2>();
        Rectangle viewport;
        int step;
        #endregion

        #region Initialization
        public Astar(BulletHell.GameStateManager gameStateManager):
            base(gameStateManager)
        {
//            graphics = new GraphicsDeviceManager(this);
//            Content.RootDirectory = "Content";
        }

        internal protected override void Initialize()
        {
            x = graphics.GraphicsDevice.Viewport.Width / 19;
            y = graphics.GraphicsDevice.Viewport.Height / 19;

            world = new Tile[x,y];
            for (int i = 0; i < x; i++)
                for (int j = 0; j < y; j++)
                    world[i, j] = new Tile(new Vector2(i * 19, j * 19), rand.NextDouble() > 1-rateOfBlocks);

            startTile = world[3, 3];
            goalTile = world[x - 4,  y-4];

            open = new List<Tile>();
            close = new List<Tile>();

            stepPos = new Vector2(750, 15);
            robPos = new Vector2(400, 15);

            viewport = new Rectangle(0, 0,
                graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height);

            fontPos = new Vector2(viewport.Width / 2 - 200,
                    viewport.Height / 2 - 10);

            //msec = 0;
            step = 0;
            path.Clear();
            restartPos = new Vector2(viewport.Width / 2 - 300,
                viewport.Height - 100);
            

            open.Add(startTile);
            LoadContent();
            //base.Initialize();
            aStar();
        }
        #endregion

        #region Contents
        internal override void LoadContent()
        {
            font = Content.Load<SpriteFont>("Astar//gamefont");
            tile = Content.Load<Texture2D>("Astar//tile");
            //spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        internal override void UnloadContent()
        {
            //tile.Dispose();
        }
        #endregion

        #region Update and Draw
        internal override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Enter) && preKeyboardState.IsKeyUp(Keys.Enter))
            //if (inputHandler.previousKeyboardState.IsKeyUp(Keys.Enter) && inputHandler.keyboardState.IsKeyDown(Keys.Enter))
            {
                //UnloadContent();
                Initialize();
            }

            //if (keyboardState.IsKeyDown(Keys.Escape) && preKeyboardState.IsKeyUp(Keys.Escape))
            //{
            //    this.Exit();
            //}

            if (keyboardState.IsKeyDown(Keys.Up) && preKeyboardState.IsKeyUp(Keys.Up))
            //if(inputHandler.isKeyUp())
                rateOfBlocks += 0.01f;

            if (keyboardState.IsKeyDown(Keys.Down) && preKeyboardState.IsKeyUp(Keys.Down))
            //if(inputHandler.isKeyDown())
                rateOfBlocks -= 0.01f; ;

            preKeyboardState = keyboardState;
            //base.Update(gameTime);

            //msec += gameTime.ElapsedRealTime.Milliseconds;

            if (/*msec % 5 < 2 && speed control*/ walker != null && walker.pre != null)
            {
                path.Add(walker.pos);
                walker = walker.pre;
                step++;
            }
        }

        internal override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            foreach (Tile coor in world)
                spriteBatch.Draw(tile, coor.pos, (coor.blocked)?Color.Gray:Color.White);

            if (walker != null && walker.pre != null)
            {
                spriteBatch.Draw(tile, walker.pos, Color.Red);
                spriteBatch.DrawString(font, "" + step, walker.pos, Color.White, 0.0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0.0f);
            }
            else
            {
                foreach (Vector2 pathTaken in path)
                {
                    spriteBatch.Draw(tile, pathTaken, Color.DarkRed);
                }
                spriteBatch.Draw(Content.Load<Texture2D>("sprites//dot"), viewport, Color.Black);
                spriteBatch.DrawString(font, "Press Enter to start another instnace\nPress up and down to configure block ratios", 
                    restartPos, Color.Red, 0.0f, Vector2.Zero, 1.2f, SpriteEffects.None, 0.0f);
            }

            spriteBatch.DrawString(font, "" + step, stepPos, Color.Blue);
            spriteBatch.DrawString(font, "Rate of Blocks: " + rateOfBlocks, robPos, Color.Blue);

            if (step == 0)
            {
                spriteBatch.DrawString(font, "Unable to go through", fontPos, Color.Red, 0.0f, 
                    Vector2.Zero,1.5f,SpriteEffects.None, 0.0f);
            }

            spriteBatch.Draw(tile, startTile.pos, Color.SkyBlue);
            spriteBatch.Draw(tile, goalTile.pos, Color.DarkRed);
            spriteBatch.End();
            //base.Draw(gameTime);
        }
        #endregion

        #region A* Algorithm and Methods
        private List<Tile> neighborTiles(Tile source)
        {
            int xCoor, yCoor;
            List<Tile> neighbors = new List<Tile>(); ;

            xCoor = (int)source.pos.X / 19;
            yCoor = (int)source.pos.Y / 19;

            // This A* takes manhattan distance approach,
            // the node can only go in 4 directions
            if (xCoor + 1 <= x - 1 && !world[xCoor + 1, yCoor].blocked)
                neighbors.Add(world[xCoor+1, yCoor]);
            if (xCoor - 1 >= 0 && !world[xCoor - 1, yCoor].blocked)
                neighbors.Add(world[xCoor-1, yCoor]);
            if (yCoor + 1 <= y - 1 && !world[xCoor, yCoor + 1].blocked)
                neighbors.Add(world[xCoor, yCoor + 1]);
            if (yCoor - 1 >= 0 && !world[xCoor, yCoor - 1].blocked)
                neighbors.Add(world[xCoor, yCoor - 1]);

            return neighbors;
        }

        // C# doesn't have a priority queue class
        // so I coded up minimum() to simulate priority ranks
        private int minimum()
        {
            int min = 9999, index=-1;
            for (int i = 0; i < open.Count; i++ )
            {
                if (open[i].rank < min)
                {
                    min = open[i].rank;
                    index = i;
                }
            }
            return index;
        }

        // cost from start to current
        private int g(Tile current){
            return (int)(Math.Abs(startTile.pos.X - current.pos.X)
                + Math.Abs(startTile.pos.Y - current.pos.Y))/19;
            //the sum of x and y coordinate divided by 19, each block is 19 by 19 pixels
        }

        // cost from src to dst
        private int movementCost(Tile src, Tile dst){
            return (int)Math.Abs((Math.Abs(src.pos.X - dst.pos.X)
                - Math.Abs(src.pos.Y - dst.pos.Y))/19);
        
        }

        // cost from src to goal
        private int h(Tile src)
        {
            return (int)Math.Abs((Math.Abs(src.pos.X - goalTile.pos.X)
                - Math.Abs(src.pos.Y - goalTile.pos.Y)))/19;

        }

        // Constructs the path in the opposite direction
        private Tile reverseConstruct(Tile src){
            Tile tmp = null;

            while (src != null)
            {
                Tile tmp2 = src.pre;
                src.pre = tmp;
                tmp = src;
                src = tmp2;
            }
            return tmp;
        }

        public void aStar()
        {
            int min = minimum(), cost;
            Tile current = new Tile(Vector2.Zero, false);
            List<Tile> neighbors;
            while (min >= 0 && open[min].pos != goalTile.pos) {
                current = open[min];
                open.RemoveAt(min);
                close.Add(current);
                neighbors = neighborTiles(current);

                foreach (Tile neighbor in neighbors) {
                    int gkost = g(current),
                        mkost = movementCost(current, neighbor),
                        gnkost = g(neighbor);
                    cost = gkost + mkost;
                    if (open.Contains(neighbor) && cost < gnkost)
                        open.Remove(neighbor);
                    if (close.Contains(neighbor) && cost < gnkost)
                        close.Remove(neighbor);
                    if (!(open.Contains(neighbor) || close.Contains(neighbor)))
                    {
                        int hkost = h(neighbor);
                        cost = gnkost;
                        neighbor.rank = gnkost + hkost;
                        neighbor.pre = current;
                        open.Add(neighbor);
                    }
                }
                min = minimum();
            }
            walker = null;
            if (min < 0) return;
            walker = open[min];
            if (walker != null)
                walker = reverseConstruct(walker);

        }
        #endregion
    }
}