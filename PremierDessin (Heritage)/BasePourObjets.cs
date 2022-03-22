using BaseOpenTk;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace PremierDessin
{
    internal abstract class BasePourObjets
    {
        #region Attributs
        protected Vector2[] listePoints;
        protected Vector2[] coordonneesTextures;
        protected int textureID;
        protected string nomTexture;
        #endregion

        #region ConstructeurInitialisateur
        public BasePourObjets(string nomTexture, Vector2 a, Vector2 b, Vector2 c)
        {
            listePoints = new Vector2[] { a, b, c };
            init(nomTexture);
            setCoordonneesTextureTriangle();
        }
        public BasePourObjets(string nomTexture, Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            listePoints = new Vector2[] { a, b, c, d };
            init(nomTexture);
            setCoordonneesTextureCarre();
        }
        private void init(string nomTexture)
        {
            this.nomTexture = nomTexture;
            chargerTexture();
        }
        #endregion

        #region GestionTexture
        private void chargerTexture()
        {
            GL.GenTextures(1, out textureID);
            GL.BindTexture(TextureTarget.Texture2D, textureID);
            BitmapData textureData = chargerImage(nomTexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, textureData.Width, textureData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgr,
                                PixelType.UnsignedByte, textureData.Scan0);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        private BitmapData chargerImage(string nomImage)
        {
            Bitmap bmpImage = new Bitmap(nomImage);
            Rectangle rectangle = new Rectangle(0, 0, bmpImage.Width, bmpImage.Height);
            BitmapData bmpData = bmpImage.LockBits(rectangle, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            bmpImage.UnlockBits(bmpData);
            return bmpData;
        }

        private void setCoordonneesTextureTriangle()
        {
            Vector2 a = new Vector2(0.05f, 1.0f);
            Vector2 b = new Vector2(1.0f, 0.95f);
            Vector2 c = new Vector2(0.55f, 0.0f);
            coordonneesTextures = new Vector2[] { a, b, c };
        }

        private void setCoordonneesTextureCarre()
        {
            Vector2 a = new Vector2(0.0f, 1.0f);
            Vector2 b = new Vector2(1.0f, 1.0f);
            Vector2 c = new Vector2(1.0f, 0.0f);
            Vector2 d = new Vector2(0.0f, 0.0f);
            coordonneesTextures = new Vector2[] { a, b, c, d };
        }
        #endregion //GestionTexture


        abstract public void update();

        abstract public Dictionary<CoteObjets, Vector2[]> getDroitesCotes();
        public void dessiner(PrimitiveType typeDessin)
        {
            GL.BindTexture(TextureTarget.Texture2D, textureID);
            GL.Begin(typeDessin);
            for (int i = 0; i < listePoints.Length; i++)
            {
                GL.TexCoord2(coordonneesTextures[i]);
                GL.Vertex2(listePoints[i].X, listePoints[i].Y);
            }
            GL.End();
        }

        protected float getAngle(Vector2 pointPrimaire, Vector2 pointSecondaire)
        {
            float angleRadian = 0.0f;
            float deltaX = pointSecondaire.X - pointPrimaire.X;
            float deltaY = pointSecondaire.Y - pointPrimaire.Y;

            if (deltaX == 0.0f)
            {
                //L'angle sera de 90 ou 270 degrés selon que DeltaY soit positif ou negatif
                if (deltaY > 0.0f)
                {
                    angleRadian = (float)Math.PI / 2.0f;
                } else
                {
                    angleRadian = (float)Math.PI * 1.5f;
                }
            }else 
            {
                angleRadian = (float)Math.Atan2(deltaY, deltaX);
            }

            return angleRadian;
        }
        protected Vector2 getPointFromAngle(float angleRadian, Vector2 pointOriginal, float facteurDistance)
        {
            float newX = (float)Math.Cos(angleRadian);
            float newY = (float)Math.Sin(angleRadian);
            newX *= facteurDistance;
            newY *= facteurDistance;
            newX += pointOriginal.X;
            newY += pointOriginal.Y;
            Math.Round(newX, 2);
            Math.Round(newY, 2);
            return new Vector2( newX, newY);
        }
    }
}
