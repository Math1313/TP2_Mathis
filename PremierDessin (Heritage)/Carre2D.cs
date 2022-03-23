using BaseOpenTk;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using PremierDessin;
using System;
using System.Collections.Generic;

namespace PremierDessin__Heritage_
{
    internal class Carre2D : BasePourObjets
    {
        float deplacementVertical;
        float incrementVertical;
        float deplacementHorizontal;
        float incrementHorizontal;
        int valDommage;
        #region ConstructeurInitialisateur
        public Carre2D(Vector2 pointA, Vector2 pointB, Vector2 pointC, Vector2 pointD) : base("./images/CaisseBoisBMP.bmp", pointA, pointB, pointC, pointD)
        {
            deplacementVertical = 0.0f;
            incrementVertical = 1.5f;
            deplacementHorizontal = 0.0f;
            incrementHorizontal = 2.0f;
            valDommage = 20;
        }
        public Carre2D(int dommage, Vector2 a, Vector2 b, Vector2 c, Vector2 d) : base("./images/CaisseBoisBMP.bmp", a, b, c, d)
        {
            deplacementVertical = 0.0f;
            deplacementHorizontal = 0.0f;
            incrementVertical = 1.5f;
            incrementHorizontal = 2.0f;
            valDommage = dommage;
        }
        #endregion

        public int getDommage()
        {
            return valDommage;
        }


        #region MethodesClasseParent
        override public void update()
        {
            if (deplacementVertical + incrementVertical >= 105.0f - listePoints[3].Y
                || deplacementVertical + incrementVertical <= -150.0f - listePoints[0].Y)
            {
                incrementVertical *= -1.0f;
            }
            deplacementVertical += incrementVertical;

            if (deplacementHorizontal + incrementHorizontal >= 300.0f - listePoints[1].X
                || deplacementHorizontal + incrementHorizontal <= -300.0f - listePoints[0].X)
            {
                incrementHorizontal *= -1.0f;
            }
            deplacementHorizontal += incrementHorizontal;
        }

        override public Dictionary<CoteObjets, Vector2[]> getDroitesCotes()
        {
            Dictionary<CoteObjets, Vector2[]> listeDroites = new Dictionary<CoteObjets, Vector2[]>();

            //Calculer les nouvelles positions des points, selon les valeurs de déplacement
            Vector2 reelPointA = new Vector2(listePoints[0].X + deplacementHorizontal, listePoints[0].Y + deplacementVertical);
            Vector2 reelPointB = new Vector2(listePoints[1].X + deplacementHorizontal, listePoints[1].Y + deplacementVertical);
            Vector2 reelPointC = new Vector2(listePoints[2].X + deplacementHorizontal, listePoints[2].Y + deplacementVertical);
            Vector2 reelPointD = new Vector2(listePoints[3].X + deplacementHorizontal, listePoints[3].Y + deplacementVertical);

            //Regrouper ces points par pair pour créer des droites, puis les ajouter au Dictionary
            listeDroites[CoteObjets.SUD] = new Vector2[] { reelPointA, reelPointB, };
            listeDroites[CoteObjets.EST] = new Vector2[] { reelPointB, reelPointC, };
            listeDroites[CoteObjets.NORD] = new Vector2[] { reelPointC, reelPointD, };
            listeDroites[CoteObjets.OUEST] = new Vector2[] { reelPointD, reelPointA, };
            return listeDroites;
        }

        public void inverserDirection(CoteObjets coteCollision)
        {
            //Les caisses restent coincé entre elle, changer le déplacement dans la classe Carre2D si la valDommage est = 10.
            if (coteCollision == CoteObjets.SUD || coteCollision.ToString() == "NORD")
            {
                incrementVertical *= -1.0f;
                deplacementVertical += incrementVertical;
                if (valDommage == 10)
                {
                }
            }
            if (coteCollision.ToString() == "OUEST" || coteCollision.ToString() == "EST")
            {
                incrementHorizontal *= -1.0f;
                deplacementHorizontal += incrementHorizontal;
                if (valDommage == 10)
                {
                }
            }
        }
        public void dessiner()
        {
            GL.PushMatrix();
            GL.Translate(deplacementHorizontal, deplacementVertical, 0.0f);

            base.dessiner(PrimitiveType.Quads);

            GL.PopMatrix();
        }

        private Vector2[] getPointsReels()
        {
            Vector2 reelPointA = new Vector2(listePoints[0].X + deplacementHorizontal, listePoints[0].Y + deplacementVertical);
            Vector2 reelPointB = new Vector2(listePoints[1].X + deplacementHorizontal, listePoints[1].Y + deplacementVertical);
            Vector2 reelPointC = new Vector2(listePoints[2].X + deplacementHorizontal, listePoints[2].Y + deplacementVertical);
            Vector2 reelPointD = new Vector2(listePoints[3].X + deplacementHorizontal, listePoints[3].Y + deplacementVertical);
            Vector2[] pointsReels = { reelPointA, reelPointB, reelPointC, reelPointD};

            return pointsReels;

        }

        public Vector2[][] getPointsPourPetitesCaisses()
        {
            Vector2[][] newPoints = new Vector2[4][];
            Vector2[] pointsReels = getPointsReels();

            float demieLongueur = (pointsReels[1].X - pointsReels[0].X) / 2;

            newPoints[0] = new Vector2[4];
            newPoints[0][0] = new Vector2(pointsReels[0].X - 3, pointsReels[0].Y - 3);
            newPoints[0][1] = new Vector2(pointsReels[0].X - 3 + demieLongueur, pointsReels[0].Y - 3);
            newPoints[0][2] = new Vector2(pointsReels[0].X - 3 + demieLongueur, pointsReels[0].Y - 3 + demieLongueur);
            newPoints[0][3] = new Vector2(pointsReels[0].X - 3, pointsReels[0].Y - 3 + demieLongueur);

            newPoints[1] = new Vector2[4];
            newPoints[1][0] = new Vector2(pointsReels[1].X + 3 - demieLongueur, pointsReels[1].Y - 3);
            newPoints[1][1] = new Vector2(pointsReels[1].X + 3, pointsReels[1].Y - 3);
            newPoints[1][2] = new Vector2(pointsReels[1].X + 3, pointsReels[1].Y - 3 + demieLongueur);
            newPoints[1][3] = new Vector2(pointsReels[1].X + 3 - demieLongueur, pointsReels[1].Y - 3 + demieLongueur);

            newPoints[2] = new Vector2[4];
            newPoints[2][0] = new Vector2(pointsReels[2].X + 3 - demieLongueur, pointsReels[2].Y + 3 - demieLongueur);
            newPoints[2][1] = new Vector2(pointsReels[2].X + 3, pointsReels[2].Y + 3 - demieLongueur);
            newPoints[2][2] = new Vector2(pointsReels[2].X + 3, pointsReels[2].Y + 3);
            newPoints[2][3] = new Vector2(pointsReels[2].X + 3 - demieLongueur, pointsReels[2].Y + 3);

            newPoints[3] = new Vector2[4];
            newPoints[3][0] = new Vector2(pointsReels[3].X - 3, pointsReels[3].Y + 3 - demieLongueur);
            newPoints[3][1] = new Vector2(pointsReels[3].X - 3 + demieLongueur, pointsReels[3].Y + 3 - demieLongueur);
            newPoints[3][2] = new Vector2(pointsReels[3].X - 3 + demieLongueur, pointsReels[3].Y + 3);
            newPoints[3][3] = new Vector2(pointsReels[3].X - 3, pointsReels[3].Y + 3);


            return newPoints;

        }

        public void getDeplacement()
        {
            Console.WriteLine("============================================");
            Console.WriteLine("Deplacement Horizontale: " + deplacementHorizontal);
            Console.WriteLine("Deplacement Verticale: " + deplacementVertical);
            Console.WriteLine("============================================");
        }
        #endregion
    }
}
