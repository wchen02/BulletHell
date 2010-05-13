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

namespace BulletHell.Game
{
    public class Map : GameState
    {
        #region Fields
        internal HeroPlaneObj hero;
        private SoundEffect BGM;
        private Texture2D background;
        internal Dictionary<string, Obj> objs;
        internal Dictionary<string, EnemyPlaneObj> enemyPlaneObjs;
        internal List<PlaneObj> enemiessToBeUpdated;
        internal List<Event> events;
        internal List<string> toBeRemoved;
        internal List<BulletObj> bulletPool;

        private String title, objective;
        private float scrollSpeed;
        private TimeSpan scrollUpdateTimer, fpsTimer, gameElapsedTimer;
        public string MSG = "";
        public string TargetShooting = "";

        public string MSG2 = "";
        int FPS = 0, frameCounter = 0;
        #endregion

        #region Constructor and Init
        public Map(String title, String objective, Texture2D background, SoundEffect BGM, float scrollSpeed, Dictionary<string, EnemyPlaneObj> enemyPlaneObjs, Dictionary<string, Obj> objs, HeroPlaneObj hero, List<Event> events)
        {
            this.title = title;
            this.objective = objective;
            this.background = background;
            this.BGM = BGM;
            this.scrollSpeed = scrollSpeed;
            this.enemyPlaneObjs = enemyPlaneObjs;
            this.objs = objs;
            this.hero = hero;
            this.events = events;
        }

        protected override void Initialize()
        {
            toBeRemoved = new List<string>();
            bulletPool = new List<BulletObj>();
            scrollUpdateTimer = new TimeSpan();
            fpsTimer = new TimeSpan();
            events = new List<Event>();
            enemiessToBeUpdated = new List<PlaneObj>();
        }
        #endregion

        #region Contents
        internal override void LoadContent()
        {
        }

        protected override void UnloadContent() { }
        #endregion

        #region Update and Draw
        internal override void Update(GameTime gameTime)
        {
            NeedUpdate = true;
            NeedDraw = true;
            hero.Update(this, gameTime);

            scrollUpdateTimer += gameTime.ElapsedGameTime;
            fpsTimer += gameTime.ElapsedGameTime;
            gameElapsedTimer += gameTime.ElapsedGameTime;

            MSG2 = bulletPool.Count + "";

            /* calcs fps */
            if (fpsTimer >= TimeSpan.FromMilliseconds(1000))
            {
                FPS = frameCounter;
                frameCounter = 0;
                fpsTimer -= TimeSpan.FromMilliseconds(1000);
            }

            /* adds the obj to be updated as the time goes */
            if (events.Count > 0) {
                for (int i = events.Count - 1; i >= 0; --i)
                {
                    if (events[i].Milliseconds < gameElapsedTimer.TotalMilliseconds)
                    {
                        enemiessToBeUpdated.Add( new EnemyPlaneObj(enemyPlaneObjs[events[i].Obj]) );
                        events.RemoveAt(i);
                    }
                }
            }

            /* updates the enempy plane objs */
            if (enemiessToBeUpdated.Count > 0)
            {
                if (scrollUpdateTimer.TotalMilliseconds > 75)
                {
                    /* moves the enemy down if scollspeed is specified */
                    for (int i = enemiessToBeUpdated.Count - 1; i >= 0; --i)
                        enemiessToBeUpdated[i].move(0.0f, scrollSpeed, enemiessToBeUpdated[i].velocity);
                    scrollUpdateTimer -= TimeSpan.FromMilliseconds(75);
                }

                for (int i = enemiessToBeUpdated.Count - 1; i >= 0; --i)
                    enemiessToBeUpdated[i].Update(this, gameTime);
            }

            /* Calling update on all of the bullets, foreach will cause a problem if the item is removed */
            for (int i = bulletPool.Count - 1; i >= 0; --i)
                bulletPool[i].Update(bulletPool, this, gameTime);

            /* Can only use foreach loop because that is the only way to iterate through dictionaries */
            foreach (string objID in toBeRemoved)
            {
                //if (enemiessToBeUpdated.Remove(objID)) continue;
                if (objs.Remove(objID)) continue;
                else if (hero.visibility == false)
                {
                    _GLOBAL.GameStateManager.activate(new Dialog("Game Over", "Please Restart", new Vector2(_GLOBAL.Viewport.Width / 2, _GLOBAL.Viewport.Height / 2)));
                    NeedUpdate = false;
                }
            }
            toBeRemoved.Clear();
        }

        internal override void Draw(GameTime gameTime)
        {
            frameCounter++;
            _GLOBAL.SpriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            _GLOBAL.SpriteBatch.Draw(background, _GLOBAL.ViewportRect, Color.White);
            foreach (Obj obj in objs.Values)
                obj.Draw(gameTime);

            foreach (EnemyPlaneObj obj in enemiessToBeUpdated)
                obj.Draw(gameTime);

            foreach (BulletObj bullet in bulletPool)
                bullet.Draw(gameTime);

            hero.Draw(gameTime);

            /* Debug messages */
            if (_GLOBAL.Debug)
            {
                _GLOBAL.SpriteBatch.DrawString(_GLOBAL.SpriteFont, TargetShooting, new Vector2(500, _GLOBAL.Viewport.Height - _GLOBAL.SpriteFont.MeasureString("\n\n\n\n").Y - 400), Color.White);
                _GLOBAL.SpriteBatch.DrawString(_GLOBAL.SpriteFont, "Bullets: " + MSG2, new Vector2(10, _GLOBAL.Viewport.Height - _GLOBAL.SpriteFont.MeasureString(" ").Y * (enemiessToBeUpdated.Count + 1)), Color.White);
                _GLOBAL.SpriteBatch.DrawString(_GLOBAL.SpriteFont, "Fps: " + FPS, new Vector2(_GLOBAL.Viewport.Width - _GLOBAL.SpriteFont.MeasureString("FPS: 60").X, 10), Color.White);
            }
            
            _GLOBAL.SpriteBatch.End();
        }
        #endregion
    }
}