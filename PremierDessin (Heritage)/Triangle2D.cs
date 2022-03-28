using BaseOpenTk;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using PremierDessin;
using System;
using System.Collections.Generic;

namespace PremierDessin__Heritage_
{
    internal class Triangle2D : BasePourObjets
    {
        float theta;
        float incrementRotation;

        #region ConstructeurInitialisateur
        public Triangle2D(Vector2 pointA, Vector2 pointB, Vector2 pointC) : base("./images/DoritosBMP.bmp", pointA, pointB, pointC)
        {
            theta = 0.0f;
            incrementRotation = 0.5f;
        }
        #endregion

        #region MethodesClasseParent
        override public void update()
        {
            theta += incrementRotation;
            if (theta >= 360.0f)
            {
                theta = 0.0f;
            }
            else if (theta <= 0.0f)
            {
                theta = 360.0f;
            }
        }

        override public Dictionary<CoteObjets, Vector2[]> getDroitesCotes()
        {
            Dictionary<CoteObjets, Vector2[]> listeDroites = new Dictionary<CoteObjets, Vector2[]>();
            float differenceX;
            float differenceY;
            float newX;
            float newY;
            float thetaRad = theta * (float)Math.PI / 180.0f;

            //Le point A est le centre donc il ne changera pas.
            Vector2 newPointA = listePoints[0];
            //Nouveau PointB
            differenceX = listePoints[1].X - listePoints[0].X;
            differenceY = listePoints[1].Y - listePoints[0].Y;
            newX = listePoints[0].X + differenceX * (float)Math.Cos(thetaRad) - differenceY * (float)Math.Sin(thetaRad);
            newY = listePoints[0].Y + differenceX * (float)Math.Sin(thetaRad) + differenceY * (float)Math.Cos(thetaRad);
            Vector2 newPointB = new Vector2(newX, newY);
            //Nouveau PointC
            differenceX = listePoints[2].X - listePoints[0].X;
            differenceY = listePoints[2].Y - listePoints[0].Y;
            newX = listePoints[0].X + differenceX * (float)Math.Cos(thetaRad) - differenceY * (float)Math.Sin(thetaRad);
            newY = listePoints[0].Y + differenceX * (float)Math.Sin(thetaRad) + differenceY * (float)Math.Cos(thetaRad);
            Vector2 newPointC = new Vector2(newX, newY);

            listeDroites[CoteObjets.SUD] = new Vector2[] { newPointA, newPointB };
            listeDroites[CoteObjets.NORD_EST] = new Vector2[] { newPointB, newPointC };
            listeDroites[CoteObjets.NORD_OUEST] = new Vector2[] { newPointC, newPointA };

            return listeDroites;
        }
        public Vector2[] getPointsSalsa()
        {
            Vector2[] listePointsSalsa = new Vector2[4];
            IDictionary<CoteObjets, Vector2[]> droitesCotes = getDroitesCotes();

            Vector2 pointMilieu = droitesCotes[CoteObjets.SUD][0];

            float angleRadian = getAngle(droitesCotes[CoteObjets.SUD][0], droitesCotes[CoteObjets.SUD][1]);
            Vector2 pointA = getPointFromAngle(angleRadian, pointMilieu, 20.0f);

            angleRadian = getAngle(droitesCotes[CoteObjets.NORD_OUEST][1], droitesCotes[CoteObjets.NORD_OUEST][0]);
            Vector2 pointB = getPointFromAngle(angleRadian, pointMilieu, 20.0f);

            float angleMilieu = getAngle(pointA, pointB) - (float)((Math.PI / 180) * 90);
            Vector2 pointC = getPointFromAngle(angleMilieu, pointB, 20.0f);
            Vector2 pointD = getPointFromAngle(angleMilieu, pointA, 20.0f);

            listePointsSalsa[0] = pointA;
            listePointsSalsa[1] = pointB;
            listePointsSalsa[2] = pointC;
            listePointsSalsa[3] = pointD;


            return listePointsSalsa;
        }

        public Vector2[][] getPointsProjectilesMinisDoritos()
        {
            //Conteneurs pour les projectiles
            Vector2[] projectileA = new Vector2[3];
            Vector2[] projectileB = new Vector2[3];
            Vector2[] projectileC = new Vector2[3];

            //Récuparations des droites du doritos avec leurs positions réelles
            IDictionary<CoteObjets, Vector2[]> droitesCotes = getDroitesCotes();
            //Calculer le projectile A
            projectileA[0] = droitesCotes[CoteObjets.SUD][0];

            float angleRadian = getAngle(droitesCotes[CoteObjets.SUD][0], droitesCotes[CoteObjets.SUD][1]);
            projectileA[1] = getPointFromAngle(angleRadian, projectileA[0], 20.0f);

            angleRadian = getAngle(droitesCotes[CoteObjets.NORD_OUEST][1], droitesCotes[CoteObjets.NORD_OUEST][0]);
            projectileA[2] = getPointFromAngle(angleRadian, projectileA[0], 20.0f);

            //Calculer le projectile B

            projectileB[0] = droitesCotes[CoteObjets.NORD_EST][0];

            angleRadian = getAngle(droitesCotes[CoteObjets.SUD][1], droitesCotes[CoteObjets.SUD][0]);
            projectileB[1] = getPointFromAngle(angleRadian, projectileB[0], 20.0f);

            angleRadian = getAngle(droitesCotes[CoteObjets.NORD_EST][0], droitesCotes[CoteObjets.NORD_EST][1]);
            projectileB[2] = getPointFromAngle(angleRadian, projectileB[0], 20.0f);

            //Calculer le projectile C

            projectileC[0] = droitesCotes[CoteObjets.NORD_OUEST][0];

            angleRadian = getAngle(droitesCotes[CoteObjets.NORD_EST][1], droitesCotes[CoteObjets.NORD_EST][0]);
            projectileC[1] = getPointFromAngle(angleRadian, projectileC[0], 20.0f);

            angleRadian = getAngle(droitesCotes[CoteObjets.NORD_OUEST][0], droitesCotes[CoteObjets.NORD_OUEST][1]);
            projectileC[2] = getPointFromAngle(angleRadian, projectileC[0], 20.0f);

            //Retourner les points des projectiles
            Vector2[][] listeProjectiles = new Vector2[3][];
            listeProjectiles[0] = projectileA;
            listeProjectiles[1] = projectileB;
            listeProjectiles[2] = projectileC;
            return listeProjectiles;
        }

        #region GestionDesEntrées
        public void inverserRotation(Key touche)
        {
            if (touche == Key.Right && incrementRotation > 0
                || touche == Key.Left && incrementRotation < 0)
            {
                incrementRotation *= -1.0f;
            }
        }
        #endregion
        public void dessiner()
        {
            GL.PushMatrix();
            GL.Translate(listePoints[0].X, 0.0f, 0.0f);
            GL.Rotate(theta, 0.0, 0.0, -1.0);
            GL.Translate(-listePoints[0].X, 0.0f, 0.0f);

            base.dessiner(PrimitiveType.Triangles);

            GL.PopMatrix();
        }
        #endregion
    }
}
