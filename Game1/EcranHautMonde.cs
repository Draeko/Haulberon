using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public class EcranHautMonde
    {
        EntiteMobile JoueurMap;
        EntiteMobile EnnemiMap;

        public EcranHautMonde(ContentManager Content, GraphicsDeviceManager graphics)
        {
            JoueurMap = new EntiteMobile(null, new Vector2(200, 200), 500f, new Rectangle());
            EnnemiMap = new EntiteMobile(null, new Vector2(graphics.PreferredBackBufferWidth - 200, graphics.PreferredBackBufferHeight - 200), 0f, new Rectangle());

            JoueurMap.Texture = Content.Load<Texture2D>("element");
            EnnemiMap.Texture = Content.Load<Texture2D>("element");
        }

        public Ecran Update(GraphicsDeviceManager graphics, GameTime gameTime)
        {
            // TODO: Add your update logic here            
            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Up))
                JoueurMap.Position.Y -= JoueurMap.Vitesse * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.Down))
                JoueurMap.Position.Y += JoueurMap.Vitesse * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.Left))
                JoueurMap.Position.X -= JoueurMap.Vitesse * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.Right))
                JoueurMap.Position.X += JoueurMap.Vitesse * (float)gameTime.ElapsedGameTime.TotalSeconds;

            JoueurMap.Position.X = Math.Min(Math.Max(JoueurMap.Texture.Width / 2, JoueurMap.Position.X), graphics.PreferredBackBufferWidth - JoueurMap.Texture.Width / 2);
            JoueurMap.Position.Y = Math.Min(Math.Max(JoueurMap.Texture.Height / 2, JoueurMap.Position.Y), graphics.PreferredBackBufferHeight - JoueurMap.Texture.Height / 2);

            return UpdateCollisions();
        }

        private Ecran UpdateCollisions()
        {
            JoueurMap.Rectangle = new Rectangle(
                (int)JoueurMap.Position.X,
                (int)JoueurMap.Position.Y,
                JoueurMap.Texture.Width,
                JoueurMap.Texture.Height);

            EnnemiMap.Rectangle = new Rectangle(
                  (int)EnnemiMap.Position.X,
                  (int)EnnemiMap.Position.Y,
                  EnnemiMap.Texture.Width,
                  EnnemiMap.Texture.Height);

            if (JoueurMap.Rectangle.Intersects(EnnemiMap.Rectangle))
            {
                return Ecran.Combat;
            }
            return Ecran.HautMonde;
        }

        public void Draw(SpriteBatch spriteBatch, ContentManager Content, GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(JoueurMap.Texture, JoueurMap.Position, null, Color.Blue, 0f,
                new Vector2(JoueurMap.Texture.Width / 2, JoueurMap.Texture.Height / 2), Vector2.One, SpriteEffects.None, 0f);
            spriteBatch.Draw(EnnemiMap.Texture, EnnemiMap.Position, null, Color.Orange, 0f,
                new Vector2(EnnemiMap.Texture.Width / 2, EnnemiMap.Texture.Height / 2), Vector2.One, SpriteEffects.None, 0f);
            spriteBatch.End();
        }
    }
}
