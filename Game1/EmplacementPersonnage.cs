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
    public class EmplacementPersonnage
    {
        public Texture2D TexturePersonnage;
        public Texture2D TexturePV;
        public Texture2D TextureCurseur;

        public Vector2 PositionPersonnage;
        public Vector2 PositionNom;
        public Vector2 PositionPVTexte;
        public Rectangle PositionPV;
        public Rectangle PositionCurseur;

        public SpriteFont Font;
        public Color ColorNom;
        public Color ColorPV;

        public Personnage Personnage;

        public EmplacementPersonnage(ContentManager Content, Personnage personnage, int PositionX, int PositionY)
        {
            this.Font = Content.Load<SpriteFont>("EcranCombat");
            this.TextureCurseur = Content.Load<Texture2D>("CurseurCombatV2");
            this.TexturePV = Content.Load<Texture2D>("pv");
            this.ColorNom = Color.White;
            this.ColorPV = Color.White;
            this.Personnage = personnage;

            this.PositionPersonnage = new Vector2(PositionX, PositionY);
            SetPositionPropriete();
        }

        private void SetPositionPropriete()
        {
            this.PositionPV = new Rectangle(
                (int)this.PositionPersonnage.X +
                (this.Personnage.Animation.Texture.Width / 2 -
                this.TexturePV.Width / 2),
                (int)this.PositionPersonnage.Y -
                this.TexturePV.Height - 5,
                this.TexturePV.Width,
                this.TexturePV.Height
                );

            this.PositionPVTexte = new Vector2(
                this.PositionPV.X +
                (this.PositionPV.Width / 2 -
                this.Font.MeasureString(this.Personnage.PvRestant + " / " + this.Personnage.Pv_max).X / 2),
                this.PositionPV.Y - 5
                );

            this.PositionNom = new Vector2(
                this.PositionPV.X +
                (this.TexturePV.Width / 2 -
                this.Font.MeasureString(Personnage.Nom).X / 2),
                this.PositionPV.Y -
                this.Font.MeasureString(Personnage.Nom).Y - 5);

            PositionCurseur = new Rectangle(
                        (int)(this.PositionNom.X
                            + Font.MeasureString(this.Personnage.Nom).X / 2
                            - TextureCurseur.Width / 2),
                        (int)this.PositionNom.Y - TextureCurseur.Height - 10,
                        TextureCurseur.Width,
                        TextureCurseur.Height);
        }

        public EmplacementPersonnage(ContentManager Content, string FontNom)
        {
            this.Font = Content.Load<SpriteFont>(FontNom);
        }

        public string ObtenirTextePV(decimal PV, int PVMax)
        {
            return PV + " / " + PVMax;
        }

        public int ObtenirPourcentagePV(decimal PV, int PVMax)
        {
            return (int)(PV / PVMax * 100);
        }
    }
}
