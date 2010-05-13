//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Media;
//using Microsoft.Xna.Framework.Net;
//using Microsoft.Xna.Framework.Storage;
//using BulletHell.System;
//using BulletHell.Game;
//using BulletHell.Game.Object;
//using BulletHell.Game.Interface;

//namespace BulletHell.Game
//{
//    public class Stage1 : GameState
//    {
//        #region Fields
//        public static string MSG = "";
//        public static List<Texture2D> sprites;
//        private static PlaneObj hero;
//        private BulletObj bullet, blueBullet;
//        private Texture2D heroSprite, enemy1Sprite, enemy2Sprite, enemy3Sprite, bulletSprite, blueBulletSprite;
//        //private TimeSpan timeElapsed;
//        private int state;
//        private List<bool> events;
//        private TimeEventHandler timeEvent;
//        #endregion

//        #region Initialization
//        public Stage1()
//        {
//        }

//        protected override void Initialize()
//        {
//            LoadContent();
//            hero = new HeroPlaneObj(heroSprite, 110, 200, 3, 3, 32, 32, 15, 100);
//            bullet = new BulletObj(bulletSprite, 1, 1, 5, 5, null, false, false, 0.0f, 0.0f);
//            blueBullet = new BulletObj(blueBulletSprite, 1, 1, 3, 3, null, true, true, 0.0f, 0.0f);

//            bullet.visibility = false;
//            hero.addBullet(bullet);
//            hero.addBullet(blueBullet);
//            SharedData.hero = hero;

//            timeEvent = new TimeEventHandler();
//            events = new List<bool>();
//            events.Add(false);
//            //timeElapsed = TimeSpan.Zero;
//            state = 0;

//            sprites = new List<Texture2D>();
//            sprites.Add(enemy1Sprite);
//            sprites.Add(enemy2Sprite);
//            sprites.Add(enemy3Sprite);
//        }
//        #endregion

//        #region Contents
//        internal override void LoadContent()
//        {
//            heroSprite = _GLOBAL.ContentManager.Load<Texture2D>(@"sprites\character\char1");
//            enemy1Sprite = _GLOBAL.ContentManager.Load<Texture2D>(@"sprites\character\char2");
//            enemy2Sprite = _GLOBAL.ContentManager.Load<Texture2D>(@"sprites\character\char3");
//            enemy3Sprite = _GLOBAL.ContentManager.Load<Texture2D>(@"sprites\character\char4");
//            bulletSprite = _GLOBAL.ContentManager.Load<Texture2D>(@"sprites\bullet");
//            blueBulletSprite = _GLOBAL.ContentManager.Load<Texture2D>(@"sprites\bluebullet");
//        }

//        protected override void UnloadContent() { }
//        #endregion

//        #region Update and Draw
//        internal override void Update(GameTime gameTime)
//        {
//            timeEvent.Update(gameTime);
//            //timeElapsed += gameTime.ElapsedGameTime;
//            MSG = "\nTime Elapsed: " + timeEvent.timeElapsed.ToString();

//            switch (state)
//            {
//                case 0:
//                    state0();
//                    break;
//                //case 1:
//                //    state1();
//                //    break;
//                //case 2:
//                //    state2();
//                //    break;
//                //case 3:
//                //    state3();
//                //    break;
//                //case 4:
//                //    state4();
//                //    break;
//                //case 5:
//                //    state5();
//                //    break;
//                default:
//                    //quits the game
//                    break;
//            }
//            if (_GLOBAL.InputHandler.keyboardState.IsKeyDown(Keys.R))
//            {
//                restart();
//            }
//        }

//        internal override void Draw(GameTime gameTime)
//        {
//            _GLOBAL.SpriteBatch.Begin(SpriteBlendMode.AlphaBlend);
//            _GLOBAL.SpriteBatch.DrawString(_GLOBAL.SpriteFont,
//                "\nhero.Pos: (" + hero.position.X + ", " + hero.position.Y + ")" +
//                "\nheroRect: x:" + hero.posRect.X + " width: " + hero.posRect.Width +
//                "\nhero.hp: " + hero.hp + ", hero.visibility: " + hero.visibility +
//                "\nhero.bulletSelection: " + hero.bulletSelection +
//                //"\nenemy.Pos: (" + enemy1.position.X + ", " + enemy1.position.Y + ") " +
//                //"\nenemyRect: x:" + enemy1.posRect.X + " width: " + enemy1.posRect.Width +
//                //"\nbulletRect: x:" + bullet.posRect.X + " width: " + bullet.posRect.Width +
//                //"\nvisibility: " + enemy1.visibility +
//                //"\ninactiveBullets: " + BulletObj.inactiveBullet.Count +
//                //"\nactiveBullets: " + BulletObj.activeBullet.Count +
//                MSG
//                ,
//                new Vector2(400, 200), Color.White);
//            _GLOBAL.SpriteBatch.End();
//        }
//        #endregion

//        public void restart()
//        {
//            SharedData.hero = null;
//            SharedData.heroBullets.Clear();
//            SharedData.enemyBullets.Clear();
//            SharedData.enemies.Clear();
//            Initialize();
//        }
//        public void state0()
//        {
//            MSG += "\nEnemies: " + SharedData.enemies.Count;
//            if (!events[0])
//            {
//                for (int i = 0; i < 4; ++i)
//                    events.Add(false);
//            }

//            if (!events[1] && timeEvent.elapsedSeconds() >= 0.5)
//            {
//                SharedData.enemies.Add(new Enemy0(500, 100));
//                SharedData.enemies[SharedData.enemies.Count - 1].addBullet(blueBullet);

//                SharedData.enemies.Add(new Enemy1(400, 300));
//                SharedData.enemies[SharedData.enemies.Count - 1].addBullet(blueBullet);

//                SharedData.enemies.Add(new Enemy2(250, 200));
//                SharedData.enemies[SharedData.enemies.Count - 1].addBullet(bullet);
//                events[1] = true;
//                MSG += "\nE1.visibility: " + SharedData.enemies[0].visibility;
//            }

//            if (events[1] && SharedData.enemies.Count == 0 && !events[2])
//            {
//                for (int i = 0; i < 5; i++)
//                {
//                    SharedData.enemies.Add(
//                        new Enemy0(100 + i * (32 - 5), 0 - i * (32 + 16)));
//                    SharedData.enemies[SharedData.enemies.Count - 1].addBullet(blueBullet);
//                    SharedData.enemies.Add(new Enemy0(
//                        _GLOBAL.Viewport.Width - 100 - i * (32 - 5),
//                            0 - i * (32 + 16)));
//                    SharedData.enemies[SharedData.enemies.Count - 1].addBullet(blueBullet);
//                }
//                events[2] = true;
//            }

//            if (events[2] && SharedData.enemies.Count == 0 && !events[3])
//            {
//                for (int i = 0; i < 5; i++)
//                {
//                    SharedData.enemies.Add(
//                        new Enemy0(100 + i * (32 - 5), 0 - i * (32 + 16)));
//                    SharedData.enemies[SharedData.enemies.Count - 1].addBullet(blueBullet);
//                    SharedData.enemies.Add(new Enemy0(
//                        _GLOBAL.Viewport.Width - 100 - i * (32 - 5),
//                            0 - i * (32 + 16)));
//                    SharedData.enemies[SharedData.enemies.Count - 1].addBullet(blueBullet);
//                }
//                events[3] = true;
//            }
//            if (events[3] && SharedData.enemies.Count == 0 && !events[4])
//            {
//                for (int i = 0; i < 5; i++)
//                {
//                    SharedData.enemies.Add(
//                        new Enemy0(100 + i * (32 - 5), 0 - i * (32 + 16)));
//                    SharedData.enemies[SharedData.enemies.Count - 1].addBullet(blueBullet);
//                    SharedData.enemies.Add(new Enemy0(
//                        _GLOBAL.Viewport.Width - 100 - i * (32 - 5),
//                            0 - i * (32 + 16)));
//                    SharedData.enemies[SharedData.enemies.Count - 1].addBullet(blueBullet);
//                }
//                events[4] = true;
//            }
//        }
//    }
//}