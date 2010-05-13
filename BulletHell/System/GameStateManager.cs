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
using BulletHell.System;
using BulletHell.Game;
using BulletHell.Game.Object;
using BulletHell.Game.Interface;

namespace BulletHell.System
{
    /* this class is reponsible for transitioning the game from one state to another. */
    public class GameStateManager : DrawableGameComponent // DrawableGameComponent's update and draw methods will be called by the game class's relevant methods.
    {
        #region class
        public List<GameState> GameStates { get; private set; }
        public List<GameState> GameStatesToUpdate { get; private set; }

        public GameStateManager(BulletHell game) : base(game) { }

        public override void Initialize()
        {
            GameStates = new List<GameState>();
            GameStatesToUpdate = new List<GameState>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            foreach (GameState gameState in GameStates)
                gameState.LoadContent();
        }

        protected override void UnloadContent()
        {
            //foreach (GameState gameState in GameStates)
            //    gameState.UnloadContent();
        }

        /* Checks all game states in array, add them to be updated if NeedUpdate is on. */
        public override void Update(GameTime gameTime)
        {
            foreach (GameState gameState in GameStates)
                if (gameState.NeedUpdate)
                    GameStatesToUpdate.Add(gameState);

            while (GameStatesToUpdate.Count > 0)
            {
                GameState gameState = GameStatesToUpdate[GameStatesToUpdate.Count - 1];
                gameState.NeedUpdate = false;
                gameState.Update(gameTime); // never directly add game state to the GameStatesToUpdate
                GameStatesToUpdate.RemoveAt(GameStatesToUpdate.Count - 1); // because they will be remove here.
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (GameState gameState in GameStates)
                if (gameState.NeedDraw)
                    gameState.Draw(gameTime);

            base.Draw(gameTime);
        }

        #endregion

        #region Methods
        private void add(GameState gameState)
        {
            if (!GameStates.Contains(gameState))
            {
                gameState.LoadContent();
                GameStates.Add(gameState);
            }
        }

        internal void activate(GameState gameState, bool activate)
        {
            gameState.NeedDraw = activate;
            gameState.NeedUpdate = activate;
            add(gameState);
        }

        internal void activate(GameState gameState)
        {
            activate(gameState, true);
        }

        internal void setUpdate(GameState gameState, bool value)
        {
            foreach (GameState gs in GameStates)
                if(gs == gameState)
                    gs.NeedUpdate = value;

            GameStatesToUpdate.Remove(gameState);
        }
        #endregion
    }
}
