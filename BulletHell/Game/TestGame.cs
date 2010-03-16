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

//namespace _BulletHell_
//{
//    public class Stage1 : GameState
//    {
//        #region Fields
//        public static string MSG = "";
//        internal static PlaneObj hero, enemy, enemy2;
//        internal MassShootingPlaneObj enemy3;
//        internal BulletObj bullet, blueBullet;
//        internal Texture2D heroSprite, bulletSprite, blueBulletSprite;
//        #endregion

//        #region Initialization
//        public Stage1(GameStateManager gameStateManager)
//            : base(gameStateManager)
//        {
//            LoadContent();
//            hero = new HeroPlaneObj(heroSprite, 110, 200, 3, 3, 32, 32, 15, 1);
//            enemy = new PlaneObj(heroSprite, 500, 120, 3, 3, 32, 32, 15, 100);
//            enemy2 = new PlaneObj(heroSprite, 200, 120, 3, 3, 32, 32, 15, 100);
//            enemy3 = new MassShootingPlaneObj(heroSprite, 110, 200, 3, 3, 32, 32, 15, 100);
//            bullet = new BulletObj(bulletSprite, 1, 1, 10, 10, null, false, true, 0.0f, 0.0f);
//            blueBullet = new BulletObj(blueBulletSprite, 1, 1, 5, 5, null, false, true, 0.0f, 0.0f);

//            bullet.visibility = false;
//            hero.addBullet(bullet);
//            //hero.addBullet(blueBullet);
//            //enemy3.addBullet(bullet);
//            enemy.addBullet(blueBullet);
//            enemy2.addBullet(blueBullet);
//            enemy3.addBullet(blueBullet);
//            SharedData.hero = hero;
//            SharedData.enemies.Add(enemy);
//            SharedData.enemies.Add(enemy2);
//            SharedData.enemies.Add(enemy3);
//        }

//        protected override void Initialize() { }
//        #endregion

//        #region Contents
//        internal override void LoadContent()
//        {
//            heroSprite = BulletHell.loader.Load<Texture2D>("sprites\character\char");
//            bulletSprite = BulletHell.loader.Load<Texture2D>("sprites\bullet");
//            blueBulletSprite = BulletHell.loader.Load<Texture2D>("sprites\bluebullet");
//        }

//        protected override void UnloadContent() { }
//        #endregion

//        #region Update and Draw
//        internal override void Update(GameTime gameTime)
//        { }

//        internal override void Draw(GameTime gameTime)
//        {
//            BulletHell.spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
//            BulletHell.spriteBatch.DrawString(_GLOBAL.SpriteBatch,
//                "\nhero.Pos: (" + hero.position.X + ", " + hero.position.Y + ")" +
//                "\nheroRect: x:" + hero.posRect.X + " width: " + hero.posRect.Width +
//                "\nenemy.Pos: (" + enemy.position.X + ", " + enemy.position.Y + ") " +
//                "\nenemyRect: x:" + enemy.posRect.X + " width: " + enemy.posRect.Width +
//                "\nbulletRect: x:" + bullet.posRect.X + " width: " + bullet.posRect.Width +
//                "\nvisibility: " + enemy.visibility +
//                //"\ninactiveBullets: " + BulletObj.inactiveBullet.Count +
//                //"\nactiveBullets: " + BulletObj.activeBullet.Count +
//                MSG
//                ,
//                new Vector2(400, 200), Color.White);
//            BulletHell.spriteBatch.End();
//        }
//        #endregion
//    }
//}