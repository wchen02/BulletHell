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
    public class SharedData
    {
        internal static PlaneObj hero;
        internal static List<PlaneObj> enemies, inactiveEnemies;
        internal static List<BulletObj> heroBullets, inactiveHeroBullets,
            enemyBullets, inactiveEnemyBullets;

        public SharedData()
        {
            hero = null;
            enemies = new List<PlaneObj>();
            heroBullets = new List<BulletObj>();
            enemyBullets = new List<BulletObj>();

            inactiveEnemies = new List<PlaneObj>();
            inactiveHeroBullets = new List<BulletObj>();
            inactiveEnemyBullets = new List<BulletObj>();
        }

        internal void Update(GameTime gameTime)
        {
            if(hero != null && hero.visibility) hero.Update(gameTime);

            foreach(PlaneObj enemy in enemies)
                if (enemy.visibility) enemy.Update(gameTime);
                else inactiveEnemies.Add(enemy); ;

            foreach (PlaneObj enemy in inactiveEnemies)
                enemies.Remove(enemy);

            inactiveEnemies.Clear();

            foreach (BulletObj bullet in heroBullets)
                if (bullet.visibility) bullet.Update(gameTime);
                else inactiveHeroBullets.Add(bullet);

            foreach (BulletObj bullet in inactiveHeroBullets)
                heroBullets.Remove(bullet);

            inactiveHeroBullets.Clear();

            foreach (BulletObj bullet in enemyBullets)
                if (bullet.visibility) bullet.Update(gameTime);
                else inactiveEnemyBullets.Add(bullet);

            foreach (BulletObj bullet in inactiveEnemyBullets)
                enemyBullets.Remove(bullet);

            inactiveEnemyBullets.Clear();
        }

        internal void Draw(GameTime gameTime)
        {
            _GLOBAL.SpriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            if (hero.visibility)
                hero.Draw(gameTime);
            for (int i = 0; i < enemies.Count; ++i)
                enemies[i].Draw(gameTime);
            for (int i = 0; i < heroBullets.Count; ++i)
                heroBullets[i].Draw(gameTime);
            for (int i = 0; i < enemyBullets.Count; ++i)
                enemyBullets[i].Draw(gameTime);
            _GLOBAL.SpriteBatch.End();
        }
    }
}