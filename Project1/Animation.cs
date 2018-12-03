using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haulberon
{
    public class Animation
    {
        public Texture2D Texture;
        public int NombreImages;
        private int imageCourante;
        public int ImageCourante
        {
            get { return imageCourante; }
            set
            {
                if (value == NombreImages) imageCourante = 0;
                else imageCourante = value;
            }
        }
        public float TempsParImage;
        public float TempsEcoule;

        public Animation(string Nom)
        {
            NombreImages = 4;
            ImageCourante = 0;
            TempsEcoule = 0;
            TempsParImage = 0.5f; 
        }

        public void Change(ContentManager Content, string Nom)
        {
            if(Texture == null)
                Texture = Content.Load<Texture2D>(Nom + "/" + Nom.ToLower() + ImageCourante);
            if (TempsEcoule > TempsParImage)
            {
                TempsEcoule = 0f;

                ImageCourante++;
                Texture = Content.Load<Texture2D>(Nom + "/" + Nom.ToLower() + ImageCourante);
            }
        }
    }
}
