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

        public EmplacementOptionMenu(ContentManager Content, string texte, int PositionTexteX, int PositionTexteY, int LargeurCurseur, int HauteurCurseur)
        {
            Font = Content.Load<SpriteFont>("OptionMenu");
            this.Texte = texte;
            PositionTexte = new Vector2(PositionTexteX, PositionTexteY);
            PositionCurseur = new Rectangle(
                (int)PositionTexte.X - LargeurCurseur - 10,
                (int)(PositionTexteY + Font.MeasureString(Texte).Y/2 - HauteurCurseur / 2),
                LargeurCurseur,
                HauteurCurseur
                );

        }
    }
}
