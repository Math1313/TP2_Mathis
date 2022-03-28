using BaseOpenTk;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using PremierDessin;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PremierDessin__Heritage_
{
    class Projectile4p : BasePourObjets
    {
        #region Attributs
        float increment_X;
        float increment_Y;
        float distanceParcourue_X;
        float distanceParcourue_Y;
        bool estActif;
        #endregion
        #region ConstructeurEtInitialisation
        public Projectile4p(string nomTexture, Vector2 pointA, Vector2 pointB, Vector2 pointC, Vector2 pointD)
        : base(nomTexture, pointA, pointB, pointC, pointD)
        {
            estActif = true;
            calculerIncrements();
        }
        private void calculerIncrements()
        {
            Vector2 pointCentreAB = getPointCentre(listePoints[0], listePoints[1]);
            Vector2 pointCentreCD = getPointCentre(listePoints[2], listePoints[3]);
            float angle = getAngle(pointCentreCD, pointCentreAB);
            Vector2 pointDirection = getPointFromAngle(angle, pointCentreAB, 2.0f);
            this.increment_X = (pointDirection.X - pointCentreAB.X);
            this.increment_Y = (pointDirection.Y - pointCentreAB.Y);
        }
        private Vector2 getPointCentre(Vector2 point_1, Vector2 point_2)
        {
            Vector2 pointCentre = new Vector2((point_1.X + point_2.X) / 2, (point_1.Y + point_2.Y) / 2);
            return pointCentre;
        }
        #endregion
        #region MethodesClasseParent
         override public void update()
         {
            if (estActif || (Math.Abs(distanceParcourue_X) < 20f && Math.Abs(distanceParcourue_Y) < 20f))
            {
                for (int i = 0; i < 4; i++)
                {
                    listePoints[i].X += increment_X;
                    listePoints[i].Y += increment_Y;
                }
                distanceParcourue_X += increment_X;
                distanceParcourue_Y += increment_Y;
            }
         }
        public void dessiner()
        {
            GL.PushMatrix();
            base.dessiner(PrimitiveType.Quads);
            GL.PopMatrix();
        }
        public override Dictionary<CoteObjets, Vector2[]> getDroitesCotes()
        {
            Dictionary<CoteObjets, Vector2[]> listeDroites = new Dictionary<CoteObjets, Vector2[]>();
            listeDroites[CoteObjets.SUD] = new Vector2[] { listePoints[0], listePoints[1] };
            listeDroites[CoteObjets.EST] = new Vector2[] { listePoints[1], listePoints[2] };
            listeDroites[CoteObjets.NORD] = new Vector2[] { listePoints[2], listePoints[3] };
            listeDroites[CoteObjets.OUEST] = new Vector2[] { listePoints[3], listePoints[0] };
            return listeDroites;
        }
        private void chargerTexture()
        {
            GL.GenTextures(1, out textureID);
            GL.BindTexture(TextureTarget.Texture2D, textureID);
            BitmapData textureData = chargerImage(nomTexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba32ui, textureData.Width, textureData.Height, 0,
            OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, textureData.Scan0);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }
        private BitmapData chargerImage(string nomImage)
        {
            Bitmap bmpImage = new Bitmap(nomImage);
            Rectangle rectangle = new Rectangle(0, 0, bmpImage.Width, bmpImage.Height);
            BitmapData bmpData = bmpImage.LockBits(rectangle, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            bmpImage.UnlockBits(bmpData);
            return bmpData;
        }
        #endregion
        public bool getEstActif()
        {
            return estActif;
        }
        public void setEstActif(bool siActif)
        {
            estActif = siActif;
        }
    }
}
