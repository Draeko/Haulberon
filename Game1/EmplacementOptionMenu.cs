using Haulberon;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public class EmplacementOptionMenu
    {
        public SpriteFont Font;
        public string Texte;
        public Vector2 PositionTexte;
        public Rectangle PositionCurseur;
        public Texture2D TextureCurseur;
        public Competence Competence;

        public EmplacementOptionMenu(ContentManager Content, string texte)
        {
            Font = Content.Load<SpriteFont>("OptionMenu");
            TextureCurseur = Content.Load<Texture2D>("CurseurMenu");
            this.Texte = texte;
        }

        public EmplacementOptionMenu(ContentManager Content, string texte, int PositionTexteX, int PositionTexteY, int LargeurCurseur, int HauteurCurseur)
        {
            Font = Content.Load<SpriteFont>("OptionMenu");
            this.Texte = texte;
            PositionTexte = new Vector2(PositionTexteX, PositionTexteY);
            PositionCurseur = new Rectangle(
                (int)PositionTexte.X - LargeurCurseur - 10,
                (int)(PositionTexte.Y + Font.MeasureString(Texte).Y/2 - HauteurCurseur / 2),
                LargeurCurseur,
                HauteurCurseur
                );

        }

        public void DefinirPositionCurseur()
        {
            PositionCurseur = new Rectangle(
                (int)PositionTexte.X - TextureCurseur.Width - 10,
                (int)(PositionTexte.Y + Font.MeasureString(Texte).Y / 2 - TextureCurseur.Height / 2),
                TextureCurseur.Width,
                TextureCurseur.Height
                );
        }
    }
}
