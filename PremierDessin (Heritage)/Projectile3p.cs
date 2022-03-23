using BaseOpenTk;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using PremierDessin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PremierDessin__Heritage_
{
    internal class Projectile3p : BasePourObjets
    {
        #region Attributs
        float increment_X = 0.0f;
        float increment_Y = 0.0f;
        float deplacementX = 0.0f;
        float deplacementY = 0.0f;
        #endregion //Attributs
        #region ConstructeurInitialisateur
        public Projectile3p(string nomTexture, Vector2 pointA, Vector2 pointB, Vector2 pointC)
            : base(nomTexture, pointA, pointB, pointC)
        {
            calculerIncrements();
        }

        private void calculerIncrements()
        {
            Vector2 pointCentreBC = getPointCentreBC();
            float angle = getAngle(pointCentreBC, listePoints[0]);
            Vector2 pointDirection = getPointFromAngle(angle, listePoints[0], 2.0f);
            this.increment_X = (pointDirection.X - listePoints[0].X);
            this.increment_Y = (pointDirection.Y - listePoints[0].Y);
        }
        private Vector2 getPointCentreBC()
        {
            Vector2 point_1 = listePoints[1]; //POINT B
            Vector2 point_2 = listePoints[2]; //POINT C
            Vector2 pointCentre = new Vector2((point_1.X + point_2.X) / 2, (point_1.Y + point_2.Y) / 2);
            return pointCentre;
        }
        #endregion //ConstructeurInitialisateur

        #region MéthodesClasseParents
        public override void update()
        {
            calculerIncrements();
            deplacementX += increment_X;
            deplacementY += increment_Y;
        }
        public void dessiner()
        {
            GL.PushMatrix();
            GL.Translate(deplacementY, deplacementX, 0.0f);
            base.dessiner(PrimitiveType.Triangles);
            GL.PopMatrix();
        }

        public Vector2 getPremierPoint()
        {
            //return listePoints[0];
            return new Vector2(listePoints[0].X - deplacementX, listePoints[0].Y - deplacementY);
        }
        public override Dictionary<CoteObjets, Vector2[]> getDroitesCotes()
        {
            Dictionary<CoteObjets, Vector2[]> listeDroites = new Dictionary<CoteObjets, Vector2[]>();
            listeDroites[CoteObjets.SUD] = new Vector2[] { listePoints[0], listePoints[1] };
            listeDroites[CoteObjets.NORD_EST] = new Vector2[] { listePoints[1], listePoints[2] };
            listeDroites[CoteObjets.NORD_OUEST] = new Vector2[] { listePoints[2], listePoints[0] };

            return listeDroites;
        }
        #endregion //MéthodesClasseParents


    }
}
