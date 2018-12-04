using Haulberon;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Game1
{
    struct EntiteMobile
    {
        public Texture2D Texture;
        public Vector2 Position;
        public float Vitesse;
        public Rectangle Rectangle;

        public EntiteMobile(Texture2D texture2D, Vector2 vector2, float f, Rectangle rectangle)
        {
            Texture = texture2D;
            Position = vector2;
            Vitesse = f;
            Rectangle = rectangle;
        }
    }


    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    /// 
    public class Game1 : Game
    {
        Random EntierAleatoire;
        Inventaire Inventaire;
        
        List<Personnage> ListePersonnages;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public Ecran EtatDuJeu;
        
        EcranHautMonde EcranMap;
        EcranCombat EcranCombat;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferHeight = 600,
                PreferredBackBufferWidth = 800
            };
            //graphics.ToggleFullScreen();
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            EntierAleatoire = new Random();
            Inventaire = new Inventaire();

            ListePersonnages = new List<Personnage>();
            Personnage Guerrier = new Personnage(1, "jouable");
            Guerrier.Animation.NombreImages = 1;
            Guerrier.Animation.Change(Content, Guerrier.Nom);
            Personnage Druide = new Personnage(4, "jouable");
            Druide.Animation.NombreImages = 1;
            Druide.Animation.Change(Content, Druide.Nom);
            Personnage Mage = new Personnage(3, "jouable");
            Mage.Animation.NombreImages = 1;
            Mage.Animation.Change(Content, Mage.Nom);


            ListePersonnages.Insert(0,Guerrier);
            ListePersonnages.Insert(0,Druide);
            ListePersonnages.Insert(0,Mage);

            Personnage Ennemi1 = new Personnage(3, "bestiaires");
            Ennemi1.Animation.TempsParImage = 0.45f;
            Ennemi1.Animation.Change(Content, Ennemi1.Nom);
            Ennemi1.PvRestant = 5;
            Personnage Ennemi2 = new Personnage(3, "bestiaires");
            Ennemi2.Animation.TempsParImage = 0.40f;
            Ennemi2.Animation.Change(Content, Ennemi2.Nom);
            Ennemi2.PvRestant = 10;
            Personnage Ennemi3 = new Personnage(3, "bestiaires");
            Ennemi3.Animation.TempsParImage = 0.43f;
            Ennemi3.Animation.Change(Content, Ennemi3.Nom);
            Ennemi3.PvRestant = 15;
            Personnage Ennemi4 = new Personnage(3, "bestiaires");
            Ennemi4.Animation.TempsParImage = 0.43f;
            Ennemi4.Animation.Change(Content, Ennemi4.Nom);

            ListePersonnages.Insert(0,Ennemi1);
            ListePersonnages.Insert(0,Ennemi2);
            ListePersonnages.Insert(0,Ennemi3);
            ListePersonnages.Insert(0,Ennemi4);
            
            
            EtatDuJeu = Ecran.HautMonde;
            EcranMap = new EcranHautMonde(Content, graphics);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            switch (EtatDuJeu)
            {
                case Ecran.HautMonde:
                    EtatDuJeu = EcranMap.Update(graphics, gameTime);
                    break;
                case Ecran.ChoixPersonnage:
                    EtatDuJeu = EcranCombat.Update(ListePersonnages, EtatDuJeu, Content);
                    break;
                case Ecran.ChoixAction:
                    EtatDuJeu = EcranCombat.Update(ListePersonnages, EtatDuJeu, Content);
                    break;
                case Ecran.ChoixCompetence:
                    EtatDuJeu = EcranCombat.Update(ListePersonnages, EtatDuJeu, Content);
                    break;
                case Ecran.ChoixCible:
                    EtatDuJeu = EcranCombat.Update(ListePersonnages, EtatDuJeu, Content);
                    break;
                case Ecran.Inventaire:
                    //EtatDuJeu = EcranInventaire.Update();
                    break;
                case Ecran.ResolutionAttaque:
                    EtatDuJeu = EcranCombat.Update(ListePersonnages, EtatDuJeu, Content);
                    break;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            switch (EtatDuJeu)
            {
                case Ecran.HautMonde:
                    EcranMap.Draw(spriteBatch, Content, gameTime);
                    break;
                case Ecran.ChoixPersonnage:
                    if (EcranCombat == null) EcranCombat = new EcranCombat(Content, graphics, ListePersonnages, EtatDuJeu);
                    EcranCombat.Draw(spriteBatch, Content, gameTime, ListePersonnages, EtatDuJeu);
                    break;
                case Ecran.ChoixAction:
                    EcranCombat.Draw(spriteBatch, Content, gameTime, ListePersonnages, EtatDuJeu);
                    break;
                case Ecran.ChoixCompetence:
                    EcranCombat.Draw(spriteBatch, Content, gameTime, ListePersonnages, EtatDuJeu);
                    break;
                case Ecran.ChoixCible:
                    EcranCombat.Draw(spriteBatch, Content, gameTime, ListePersonnages, EtatDuJeu);
                    break;
                case Ecran.Inventaire:
                    //if (EcanInventaire == null) EcranInventaire = new EcranInventaire();
                    //EcranInventaire.Draw();
                    break;
                case Ecran.ResolutionAttaque:
                    EcranCombat.Draw(spriteBatch, Content, gameTime, ListePersonnages, EtatDuJeu);
                    break;
            }
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
