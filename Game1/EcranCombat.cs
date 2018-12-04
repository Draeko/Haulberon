using Haulberon;
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
    public struct Option
    {
        public SpriteFont Font;
        public string Texte;
        public Vector2 Position;
        public Color Couleur;

        public Option(SpriteFont OptionFont, string OptionTexte, Vector2 OptionPosition, Color OptionCouleur)
        {
            Font = OptionFont;
            Texte = OptionTexte;
            Position = OptionPosition;
            Couleur = OptionCouleur;
        }
    }

    public class EcranCombat
    {
        public SpriteFont Font;

        public List<EmplacementPersonnage> ListeEmplacementPersonnages;
        public List<EmplacementOptionMenu> ListeEmplacementOptionMenu;
        #region Fenetre du combat
        public Rectangle RectangleFenetreEnnemis;
        public Texture2D TextureFenetreEnnemis;
        private Point DepartFenetreEnnemis;

        

        public Texture2D CurseurCombat;
        public Rectangle PositionCurseurCombat;
        public int EtatCurseurCombat;
        #endregion

        
        public Rectangle RectangleFenetreMenu;
        public Texture2D TextureFenetreMenu;
        private Point DepartFenetreMenu;
        
        public List<Option> Options;

        KeyboardState PrecedentEtatClavier;

        public Texture2D Curseur;
        public Rectangle PositionCurseur;
        public int EtatCurseur;

        public enum Action
        {
            Attaquer = 0,
            Inventaire = 1,
            Menu = 2,
            Retour = 3
        };
        public int ActionChoisie;

        #region Constructeur
        public EcranCombat(ContentManager Content, GraphicsDeviceManager graphics, IList<Personnage> ListePersonnages, Ecran EtatDuJeu)
        {
            Font = Content.Load<SpriteFont>("EcranCombat");
            Curseur = Content.Load<Texture2D>("CurseurMenu");
            CurseurCombat = Content.Load<Texture2D>("curseurCombatV2");
            TextureFenetreMenu = Content.Load<Texture2D>("FenetreEcranCombat");
            TextureFenetreEnnemis = Content.Load<Texture2D>("FenetreEcranCombat");
            EtatCurseur = 0;
            PrecedentEtatClavier = new KeyboardState();
            ActionChoisie = -1;
            ListeEmplacementPersonnages = new List<EmplacementPersonnage>();
            ListeEmplacementOptionMenu = new List<EmplacementOptionMenu>();

            //Fenetre du combat
            DepartFenetreEnnemis = new Point(0, 0);
            int WidthFenetreEnnemis = graphics.PreferredBackBufferWidth;
            int HeightFenetreEnemis = graphics.PreferredBackBufferHeight * 3 / 4;
            RectangleFenetreEnnemis = new Rectangle(
                DepartFenetreEnnemis.X, 
                DepartFenetreEnnemis.Y, 
                WidthFenetreEnnemis, 
                HeightFenetreEnemis);
            
            int NombrePersonnageAdverse = ListePersonnages.Count(p => p.EstAdversaire == true);            
            int NombrePersonnageControle = ListePersonnages.Count(p => p.EstAdversaire == false);            
            foreach (Personnage personnage in ListePersonnages)
            {
                int a, b, x, y;
                
                if (personnage.EstAdversaire)
                {
                    int NombreEmplacementUtilise = ListeEmplacementPersonnages.Count(e => e.Personnage.EstAdversaire == true);
                    a = RectangleFenetreEnnemis.Width / NombrePersonnageAdverse;
                    b = NombrePersonnageAdverse - NombreEmplacementUtilise;                    
                    y = RectangleFenetreEnnemis.Height / 3;
                }
                else
                {
                    int NombreEmplacementUtilise = ListeEmplacementPersonnages.Count(e => e.Personnage.EstAdversaire == false);
                    a = RectangleFenetreEnnemis.Width / NombrePersonnageControle;
                    b = NombrePersonnageControle - NombreEmplacementUtilise;
                    y = RectangleFenetreEnnemis.Height * 3 / 4;
                }

                x = (a * b - a / 2) - personnage.Animation.Texture.Width / 2;

                EmplacementPersonnage emplacement = new EmplacementPersonnage(Content, personnage, x, y);
                ListeEmplacementPersonnages.Add(emplacement);
            }

            //position du curseurCombat
            ObtenirPositionCurseurCombat(EtatDuJeu);

            //Fenetre Menu occupe le quart inférieur de l'écran
            DepartFenetreMenu = new Point(0, graphics.PreferredBackBufferHeight * 3 / 4);
            int Width = graphics.PreferredBackBufferWidth;
            int Height = graphics.PreferredBackBufferHeight / 4;
            RectangleFenetreMenu = new Rectangle(DepartFenetreMenu.X, DepartFenetreMenu.Y, Width, Height);

            ListeEmplacementOptionMenu.Add(new EmplacementOptionMenu(Content, "Attaquer", 
                DepartFenetreMenu.X + RectangleFenetreMenu.Width / 3, 
                DepartFenetreMenu.Y + RectangleFenetreMenu.Height / 3,
                Curseur.Width, 
                Curseur.Height));
            ListeEmplacementOptionMenu.Add(new EmplacementOptionMenu(Content, "Inventaire",
                DepartFenetreMenu.X + RectangleFenetreMenu.Width * 2/3,
                DepartFenetreMenu.Y + RectangleFenetreMenu.Height / 3, 
                Curseur.Width, 
                Curseur.Height));
            ListeEmplacementOptionMenu.Add(new EmplacementOptionMenu(Content, "Menu",
                DepartFenetreMenu.X + RectangleFenetreMenu.Width / 3,
                DepartFenetreMenu.Y + RectangleFenetreMenu.Height * 2/3,
                Curseur.Width, 
                Curseur.Height));
            ListeEmplacementOptionMenu.Add(new EmplacementOptionMenu(Content, "Retour",
                DepartFenetreMenu.X + RectangleFenetreMenu.Width * 2/3,
                DepartFenetreMenu.Y + RectangleFenetreMenu.Height * 2/3, 
                Curseur.Width, 
                Curseur.Height));
            PositionCurseur = ListeEmplacementOptionMenu[0].PositionCurseur;
                
        }
        #endregion

        private void DefinirPositionDesActions()
        {
            int a = RectangleFenetreMenu.Width / 3;
            int b = RectangleFenetreMenu.Height / 3;
            int compteurX = 1;
            int compteurY = 1;
            foreach (EmplacementOptionMenu option in ListeEmplacementOptionMenu)
            {
                if (compteurX == 3)
                { compteurX = 1;
                    compteurY++;
                }
                int x = DepartFenetreMenu.X + a * compteurX;
                int y = DepartFenetreMenu.Y + b * compteurY;

                option.PositionTexte = new Vector2(x,y);
                option.DefinirPositionCurseur(Curseur.Width, Curseur.Height);
                compteurX++;
            }
        }
        #region Draw
        public void Draw(SpriteBatch spriteBatch, ContentManager Content, GameTime gameTime, IEnumerable<Personnage> ListePersonnages, Ecran EtatDuJeu)
        {
            spriteBatch.Begin();
            //Affichage espace scène de combat
            spriteBatch.Draw(TextureFenetreEnnemis, RectangleFenetreEnnemis, Color.ForestGreen);
            //Affichage espace menu
            spriteBatch.Draw(TextureFenetreMenu, RectangleFenetreMenu, Color.RoyalBlue);           
            //Affichage des personnages
            DrawPersonnages(spriteBatch, Content, gameTime, ListePersonnages);
            //Affichage du curseurCombat
            DrawCurseurCombat(spriteBatch, Content, gameTime);
            //Affichage du Menu
            DrawMenu(spriteBatch, Content, gameTime, EtatDuJeu);
            spriteBatch.End();
        }

        private void DrawMenu(SpriteBatch spriteBatch, ContentManager Content, GameTime gameTime, Ecran EtatDuJeu)
        {
            switch (EtatDuJeu)
            {
                case Ecran.ChoixPersonnage:
                    DrawChoixPersonnage(spriteBatch);                    
                    break;
                case Ecran.ChoixAction:
                    DrawChoixAction(spriteBatch);                    
                    break;
                case Ecran.ChoixCompetence:
                    DrawChoixCompetence(spriteBatch, Content);
                    break;
                //Parcourir Inventaire
                //Parcourir Compétences
            }
        }

        private void DrawChoixCompetence(SpriteBatch spriteBatch, ContentManager Content)
        {
            Personnage Personnage = ListeEmplacementPersonnages.Where(e => e.Personnage.EstAdversaire == false).ToList()[EtatCurseurCombat].Personnage;
            //Affichage Nom du Personnage selectionné
            spriteBatch.DrawString(Font,
                Personnage.Nom,
                new Vector2(
                    DepartFenetreMenu.X + 50,
                    DepartFenetreMenu.Y + 10),
                Color.White);
            //Affichage Competences
            
            foreach(EmplacementOptionMenu option in ListeEmplacementOptionMenu)
            {
                spriteBatch.DrawString(option.Font,
                    option.Texte,
                    option.PositionTexte,
                    Color.White);
            }
            DrawCurseur(spriteBatch);
        }

        private void DrawChoixPersonnage(SpriteBatch spriteBatch)
        {
            string texte = "Selection du personnage.";
            spriteBatch.DrawString(Font,
                texte,
                new Vector2(
                    DepartFenetreMenu.X + (RectangleFenetreMenu.Width / 2
                    - Font.MeasureString(texte).X / 2),
                    DepartFenetreMenu.Y + (RectangleFenetreMenu.Height / 2)
                    - Font.MeasureString(texte).Y / 2),
                Color.White);
        }

        private void DrawChoixAction(SpriteBatch spriteBatch)
        {
            string NomPersonnage = ListeEmplacementPersonnages.Where(e => e.Personnage.EstAdversaire == false).ToList()[EtatCurseurCombat].Personnage.Nom;
            //Affichage Nom du Personnage selectionné
            spriteBatch.DrawString(Font,
                NomPersonnage,
                new Vector2(
                    DepartFenetreMenu.X + 50,
                    DepartFenetreMenu.Y + 10),
                Color.White);
            //Affichage Actions
            foreach (EmplacementOptionMenu option in ListeEmplacementOptionMenu)
            {
                spriteBatch.DrawString(option.Font,
                    option.Texte,
                    option.PositionTexte,
                    Color.White);
            }
            DrawCurseur(spriteBatch);
        }

        private void DrawPersonnages(SpriteBatch spriteBatch, ContentManager Content, GameTime gameTime, IEnumerable<Personnage> ListePersonnages)
        {
            foreach(Personnage personnage in ListePersonnages)
            {
                EmplacementPersonnage emplacement = ListeEmplacementPersonnages.Find(x=>x.Personnage == personnage);
                if (emplacement != null)
                {
                    personnage.Animation.TempsEcoule += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    personnage.Animation.Change(Content, personnage.Nom);

                    //Affichage de la texture du personnage
                    spriteBatch.Draw(
                        personnage.Animation.Texture,
                        emplacement.PositionPersonnage,
                        Color.White);

                    //Affichage du nom
                    spriteBatch.DrawString(
                        Font,
                        personnage.Nom,
                        emplacement.PositionNom,
                        emplacement.ColorNom);

                    //Affichage du fond de la barre de PV
                    spriteBatch.Draw(
                        emplacement.TexturePV,
                        emplacement.PositionPV,
                        Color.Black
                        );
                    //Affichage de la barre de PV
                    spriteBatch.Draw(
                        emplacement.TexturePV,
                        new Rectangle(
                            emplacement.PositionPV.X,
                            emplacement.PositionPV.Y,
                            emplacement.ObtenirPourcentagePV(personnage.PvRestant, personnage.Pv_max),
                            emplacement.PositionPV.Height),
                        Color.White
                        );

                    //Affichage des PV en texte
                    spriteBatch.DrawString(
                        Font,
                        emplacement.ObtenirTextePV(personnage.PvRestant, personnage.Pv_max),
                        emplacement.PositionPVTexte,
                        Color.White
                        );
                }
            }
        }

        private void DrawCurseurCombat(SpriteBatch spriteBatch, ContentManager Content, GameTime gameTime)
        {
            spriteBatch.Draw(CurseurCombat, PositionCurseurCombat, Color.White);
        }

        private void DrawCurseur(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Curseur, PositionCurseur, Color.White);
        }


        #endregion

        #region Update
        public Ecran Update(IEnumerable<Personnage> ListePersonnages, Ecran EtatDuJeu, ContentManager Content)
        {
            switch(EtatDuJeu)
            {
                case Ecran.ChoixPersonnage:
                    return UpdateSelectionEntite(ListePersonnages.Where(p => p.EstAdversaire == false), EtatDuJeu);
                case Ecran.ChoixAction:
                    return UpdateSelectionOption(EtatDuJeu);
                case Ecran.ChoixCompetence:
                    Personnage Personnage = ListeEmplacementPersonnages.Where(e => e.Personnage.EstAdversaire == false).ToList()[EtatCurseurCombat].Personnage;
                    ListeEmplacementOptionMenu = new List<EmplacementOptionMenu>();
                    foreach (Competence competence in Personnage.ListeCompetences)
                    {
                        ListeEmplacementOptionMenu.Add(new EmplacementOptionMenu(Content, competence.Nom));
                    }
                    DefinirPositionDesActions();
                    return UpdateSelectionOption(EtatDuJeu);
            }
            return EtatDuJeu;            
        }

        private Ecran UpdateSelectionEntite(IEnumerable<Personnage> ListeCibles, Ecran EtatDuJeu)
        {
            KeyboardState EtatClavier = Keyboard.GetState();
            #region Saisie Claiver
            if (EtatClavier.IsKeyDown(Keys.Left) && PrecedentEtatClavier.IsKeyUp(Keys.Left))
            {
                if (EtatCurseurCombat < ListeCibles.Count() - 1)
                {
                    EtatCurseurCombat++;
                    ObtenirPositionCurseurCombat(EtatDuJeu);
                }
            }

            if (EtatClavier.IsKeyDown(Keys.Right) && PrecedentEtatClavier.IsKeyUp(Keys.Right))
            {
                if (EtatCurseurCombat > 0)
                {
                    EtatCurseurCombat--;
                    ObtenirPositionCurseurCombat(EtatDuJeu);
                }
            }
            #endregion

            if (EtatClavier.IsKeyDown(Keys.Enter) && PrecedentEtatClavier.IsKeyUp(Keys.Enter))
            {
                switch (EtatDuJeu)
                {
                    case Ecran.ChoixPersonnage:
                        PrecedentEtatClavier = EtatClavier;
                        return Ecran.ChoixAction;
                }
            }
            PrecedentEtatClavier = EtatClavier;
            return EtatDuJeu;
        }

        private Ecran UpdateSelectionOption(Ecran EtatDuJeu)
        {
            KeyboardState EtatClavier = Keyboard.GetState();

            #region Saisie Clavier
            if (EtatClavier.IsKeyDown(Keys.Left) && PrecedentEtatClavier.IsKeyUp(Keys.Left))
            {
                // X--
                var L = ListeEmplacementOptionMenu.Where(x => x.PositionCurseur.X < PositionCurseur.X && x.PositionCurseur.Y == PositionCurseur.Y).OrderBy(o => o.PositionCurseur.X).Reverse().ToList();
                if (L.Count > 0)
                {
                    PositionCurseur = L[0].PositionCurseur;
                }
            }

            if (EtatClavier.IsKeyDown(Keys.Right) && PrecedentEtatClavier.IsKeyUp(Keys.Right))
            {
                // X++
                var L = ListeEmplacementOptionMenu.Where(x => x.PositionCurseur.X > PositionCurseur.X && x.PositionCurseur.Y == PositionCurseur.Y).OrderBy(o => o.PositionCurseur.X).ToList();
                if (L.Count > 0)
                {
                    PositionCurseur = L[0].PositionCurseur;
                }
            }

            if (EtatClavier.IsKeyDown(Keys.Down) && PrecedentEtatClavier.IsKeyUp(Keys.Down))
            {
                // Y++
                var L = ListeEmplacementOptionMenu.Where(x => x.PositionCurseur.Y > PositionCurseur.Y && x.PositionCurseur.X == PositionCurseur.X).OrderBy(o => o.PositionCurseur.Y).ToList();
                if (L.Count > 0)
                {
                    PositionCurseur = L[0].PositionCurseur;
                }
            }

            if (EtatClavier.IsKeyDown(Keys.Up) && PrecedentEtatClavier.IsKeyUp(Keys.Up))
            {
                // Y--
                var L = ListeEmplacementOptionMenu.Where(x => x.PositionCurseur.Y < PositionCurseur.Y && x.PositionCurseur.X == PositionCurseur.X).OrderBy(o => o.PositionCurseur.Y).Reverse().ToList();
                if (L.Count > 0)
                {
                    PositionCurseur = L[0].PositionCurseur;
                }
            }
            #endregion

            if (EtatClavier.IsKeyDown(Keys.Enter) && PrecedentEtatClavier.IsKeyUp(Keys.Enter))
            {
                PrecedentEtatClavier = EtatClavier;
                switch (EtatDuJeu)
                {
                    case Ecran.ChoixAction:
                        switch(ListeEmplacementOptionMenu.Where(x=>x.PositionCurseur == PositionCurseur).ToList()[0].Texte)
                        {
                            case "Attaquer": return Ecran.ChoixCompetence;
                            case "Inventaire": return Ecran.Inventaire; 
                            case "Menu": return Ecran.Menu;
                            case "Retour": return Ecran.ChoixPersonnage;
                        }
                        break;
                }
            }
            PrecedentEtatClavier = EtatClavier;
            return EtatDuJeu;
        }
        #endregion


        public void ObtenirPositionCurseurCombat(Ecran EtatDuJeu)
        {
            switch(EtatDuJeu)
            {
                case Ecran.ChoixPersonnage:
                    EmplacementPersonnage emplacement = ListeEmplacementPersonnages.Where(e => e.Personnage.EstAdversaire == false).ToList()[EtatCurseurCombat];
                    PositionCurseurCombat = new Rectangle(
                        (int)(emplacement.PositionNom.X
                            + Font.MeasureString(emplacement.Personnage.Nom).X / 2
                            - CurseurCombat.Width / 2),
                        (int)emplacement.PositionNom.Y - CurseurCombat.Height - 10,
                        CurseurCombat.Width,
                        CurseurCombat.Height);
                    break;                    
            }
        }

        //private void ChoixDuPersonnage(IList<Personnage> Joueurs)
        //{
        //    PositionCurseur = new Rectangle(DepartFenetreMenu.X, DepartFenetreMenu.Y + 22, Curseur.Width, Curseur.Height);
        //}

        //public void ChoixDeLAction()
        //{
        //    EtatCurseur = 0;
        //    Options = new List<Option>()
        //    {
        //        new Option(Font, "Choix de l'action : ", new Vector2(DepartFenetreMenu.X+20, DepartFenetreMenu.Y), Color.White)
        //    };

        //    int y = Curseur.Height;
        //    Options.Add(new Option(Font, "Attaquer", new Vector2(DepartFenetreMenu.X + 50, DepartFenetreMenu.Y + y), Color.White));
        //    Options.Add(new Option(Font, "Inventaire", new Vector2(DepartFenetreMenu.X + 50, DepartFenetreMenu.Y + (y*2)), Color.White));
        //    Options.Add(new Option(Font, "Retour", new Vector2(DepartFenetreMenu.X + 50, DepartFenetreMenu.Y + (y*3)), Color.White));
                
        //    PositionCurseur = new Rectangle(DepartFenetreMenu.X, DepartFenetreMenu.Y + 22, Curseur.Width, Curseur.Height);
        //}

        //public void ChoixDeLaCompetence()
        //{
        //    EtatCurseur = 0;
        //    Options = new List<Option>()
        //    {
        //        new Option(Font, "Choix de la competence : ", new Vector2(DepartFenetreMenu.X+20, DepartFenetreMenu.Y), Color.White)
        //    };
        //    int y = Curseur.Height;
        //    foreach(Competence competence in Joueur.ListeCompetences)
        //    {
        //        Options.Add(new Option(Font, competence.Nom + " - " + competence.Description, new Vector2(DepartFenetreMenu.X+Curseur.Width, DepartFenetreMenu.Y + y), Color.White));
        //        y += Curseur.Height;
        //    }

        //    PositionCurseur = new Rectangle(DepartFenetreMenu.X, DepartFenetreMenu.Y + 22, Curseur.Width, Curseur.Height);
        //}

        //public void ChoixDeLaCible(IList<Personnage> Ennemis)
        //{
        //    if(Ennemis.Count > 1)
        //    {
        //        PositionCurseur = new Rectangle(50, 50, Curseur.Width, Curseur.Height);
        //    }
        //    else { Ennemi = Ennemis[0]; }
        //}
    }
}
