using System;
using BaseOpenTk;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using PremierDessin__Heritage_;
using OpenTK.Input;
using System.Collections.Generic;
using System.Drawing;

namespace BaseOpenTk
{
    enum CoteObjets { NULL, NORD, SUD, EST, OUEST, NORD_EST, NORD_OUEST };
    internal class GestionJeu
    {
        #region Attributs
        GameWindow window;
        Triangle2D doritos;
        List<Carre2D> caissesDeBois;
        GestionAudio audio;
        Texte texteVolume;
        int valVolumeMusique;
        string txtVolumeMusique;
        bool minisDoritosEnAction;
        bool salsaEnAction;
        bool isDoritosStunned;
        bool isGameOver;
        bool isWinner;
        bool jeuActif;
        Texte textePointsDeVie;
        int valPointsDeVie;
        string stringPointsDeVie;
        Texte texteQteMinisDoritos;
        int valQteMinisDoritos;
        string stringQteMinisDoritos;
        Texte texteQteSalsa;
        int valQteSalsa;
        string stringQteSalsa;
        List<Projectile3p> minisDoritos;
        #endregion //Attributs

        #region ConstructeurInitialisateur
        public GestionJeu(GameWindow window)
        {
            this.window = window;
            /*------------Bool du Jeu-----------*/
            minisDoritosEnAction = false;
            salsaEnAction = false;
            isDoritosStunned = false;
            isGameOver = false;
            isWinner = false;
            jeuActif = true;
            /*----------------------------------*/
            start();
        }
        private void start()
        {
            double nbrImagesParSeconde = 60.0;
            double dureeAffichageChaqueImage = 1.0 / nbrImagesParSeconde;
            /*-------------------Abonnement------------------*/
            window.Load += chargement;
            window.Resize += redimensionner;
            window.UpdateFrame += update;
            window.RenderFrame += rendu;
            //window.KeyPress += actionKeyPress;
            window.KeyDown += actionKeyDown;
            //window.MouseMove += mouvemenSouris;
            window.MouseWheel += rouletteSouris;
            window.MouseDown += boutonSourisDown;
            window.MouseUp += boutonSourisUp;
            /*-----------------------------------------------*/
            window.Run(dureeAffichageChaqueImage);
        }
        private void chargement(object sender, EventArgs arg)
        {
            GL.ClearColor(0.75f, 0.75f, 0.75f, 1.0f);
            GL.Enable(EnableCap.Texture2D);
            /*------------------Triangle-----------------*/
            Vector2 pointA = new Vector2(-100.0f, 0.0f);
            Vector2 pointB = new Vector2(0.0f, 0.0f);
            Vector2 pointC = new Vector2(-50.0f, 85.0f);
            doritos = new Triangle2D(pointA, pointB, pointC);
            /*-------------------------------------------*/
            /*--------------------Carré------------------*/
            Vector2 pointD = new Vector2(40.0f, -40.0f);
            Vector2 pointE = new Vector2(100.0f, -40.0f);
            Vector2 pointF = new Vector2(100f, 20.0f);
            Vector2 pointG = new Vector2(40.0f, 20.0f);
            caissesDeBois = new List<Carre2D>();
            caissesDeBois.Add(new Carre2D(pointD, pointE, pointF, pointG));
            /*-------------------------------------------*/
            /*---------------------Audio-----------------*/
            audio = new GestionAudio();
            audio.demarrerMusiqueDeFond();
            /*-------------------------------------------*/
            /*---------------------Texte-----------------*/
            /*------------------Bool du Jeu--------------*/
            valVolumeMusique = 100;
            txtVolumeMusique = "Volume musique: ";
            valPointsDeVie = 7000;
            stringPointsDeVie = "Points de vie: ";
            valQteMinisDoritos = 20;
            stringQteMinisDoritos = "Quantité de doritos: ";
            valQteSalsa = 5;
            stringQteSalsa = "Quantité de salsa: ";
            minisDoritos = null;

            /*------------ZoneTextePointsDeVie-----------*/
            int largeurZoneTexte = 175;
            int hauteurZoneTexte = 25;
            Vector2 coinInferieurGauche = new Vector2(-293.0f, 120.0f);
            textePointsDeVie = new Texte(coinInferieurGauche, largeurZoneTexte, hauteurZoneTexte);
            /*-------------ZoneTexteQteDoritos-----------*/
            largeurZoneTexte = 215;
            coinInferieurGauche = new Vector2(-118.0f, 120.0f);
            texteQteMinisDoritos = new Texte(coinInferieurGauche, largeurZoneTexte, hauteurZoneTexte);
            /*--------------ZoneTexteQteSalsa------------*/
            largeurZoneTexte = 195;
            coinInferieurGauche = new Vector2(77.0f, 120f);
            texteQteSalsa = new Texte(coinInferieurGauche, largeurZoneTexte, hauteurZoneTexte);
            /*-------------------------------------------*/
        }
        #endregion //ConstructeurInitialisateur

        #region GestionAffichage
        private void update(object sender, EventArgs arg)
        {
            detectionCollisions();

            KeyboardState etatClavier = Keyboard.GetState();
            if (valPointsDeVie <= 0)
            {
                //if (valVolumeMusique != 0)
                //{
                //    valVolumeMusique -= 1;
                //    audio.setVolumeMusique(valVolumeMusique);
                //}
                isWinner = false;
                isGameOver = true;
                if (isGameOver && jeuActif)
                {
                    audio.jouerGameover();
                    jeuActif = false;
                    if (!isWinner)
                    {
                        audio.jouerDefaite();
                    }
                }
            }
            if (!isGameOver && !isWinner)
            {
                textePointsDeVie.setCouleurTexte(Color.Honeydew);
                textePointsDeVie.setCouleurFond(Color.Chocolate);
                textePointsDeVie.setPoliceGras();
                textePointsDeVie.setTexte(getTxtCompletPointsDeVie());

                texteQteMinisDoritos.setCouleurTexte(Color.Honeydew);
                texteQteMinisDoritos.setCouleurFond(Color.Chocolate);
                texteQteMinisDoritos.setPoliceGras();
                texteQteMinisDoritos.setTexte(getTxtCompletQteMinisDoritos());

                texteQteSalsa.setCouleurTexte(Color.Honeydew);
                texteQteSalsa.setCouleurFond(Color.Chocolate);
                texteQteSalsa.setPoliceGras();
                texteQteSalsa.setTexte(getTxtCompletQteSalsa());

                audio.setVolumeMusique(valVolumeMusique);
                foreach (Carre2D caisse in caissesDeBois)
                {
                    caisse.update();
                }

                if (etatClavier.IsKeyDown(Key.RControl) || etatClavier.IsKeyDown(Key.LControl))
                {
                    doritos.update();
                }
            }
            if (minisDoritos != null)
            {
                foreach (Projectile3p miniDoritos in minisDoritos)
                {
                    miniDoritos.update();
                }
            }
        }
        private void rendu(object sender, EventArgs arg)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            if (jeuActif)
            {
                doritos.dessiner();

                foreach (Carre2D caisse in caissesDeBois)
                {
                    caisse.dessiner();
                }
                if (minisDoritos != null)
                {
                    foreach (Projectile3p miniDoritos in minisDoritos)
                    {
                        miniDoritos.dessiner();
                    }
                }
            }

            textePointsDeVie.dessiner();
            texteQteMinisDoritos.dessiner();
            texteQteSalsa.dessiner();
            window.SwapBuffers();
        }
        private void redimensionner(object sender, EventArgs arg)
        {
            GL.Viewport(0, 0, window.Width, window.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-300.0, 300.0, -150.0, 150.0, -1.0, 1.0);
            GL.MatrixMode(MatrixMode.Modelview);
        }
        #endregion //GestionAffichage

        #region GestionDesEntrees
        private void actionKeyPress(object sender, KeyPressEventArgs arg)
        {
            Console.WriteLine("La touche appuyé est: " + arg.KeyChar.ToString());
        }

        private void actionKeyDown(object sender, KeyboardKeyEventArgs arg)
        {
            if (!isGameOver && !isWinner && !isDoritosStunned)
            {
                doritos.inverserRotation(arg.Key);
            }
        }

        private void mouvemenSouris(object sender, MouseMoveEventArgs arg)
        {
            Console.WriteLine("Position de la souris: (" + arg.X + ", " + arg.Y + ")");
        }

        private void rouletteSouris(object sender, MouseWheelEventArgs arg)
        {
            //Console.WriteLine("Delta: " + arg.Delta + " / Valeur " + arg.Value);
            valVolumeMusique += arg.Delta;
            if (valVolumeMusique < 0)
            {
                valVolumeMusique = 0;
            }
            else if (valVolumeMusique > 100)
            {
                valVolumeMusique = 100;
            }
        }

        private void boutonSourisDown(object sender, MouseButtonEventArgs arg)
        {
            if (!isGameOver && !isWinner && !isDoritosStunned)
            {
                if (arg.Button == MouseButton.Left && !audio.effetSonoreEstEnTrainDeJouer())
                {
                    if (valQteMinisDoritos <= 0)
                    {
                        audio.jouerSonPleures();
                    }
                    else
                    {
                        Vector2[][] points = doritos.getPointsProjectilesMinisDoritos();
                        minisDoritos = new List<Projectile3p>();
                        minisDoritos.Add(new Projectile3p("./images/DoritosBMP.bmp", points[0][0], points[0][1], points[0][2]));
                        minisDoritos.Add(new Projectile3p("./images/DoritosBMP.bmp", points[1][0], points[1][1], points[1][2]));
                        minisDoritos.Add(new Projectile3p("./images/DoritosBMP.bmp", points[2][0], points[2][1], points[2][2]));

                        audio.jouerSonPewpew();
                        valQteMinisDoritos -= 1;
                    }
                }
            }
        }

        private void boutonSourisUp(object sender, MouseButtonEventArgs arg)
        {
            if (!isGameOver && !isWinner && !isDoritosStunned)
            {
                if (arg.Button == MouseButton.Right && !audio.effetSonoreEstEnTrainDeJouer())
                {
                    if (valQteSalsa <= 0)
                    {
                        audio.jouerSonPleures();
                    }
                    else
                    {
                        audio.jouerSonSplash();
                        valQteSalsa -= 1;
                    }
                }
            }
        }
        #endregion //GestionDesEntrees

        private string getTxtCompletVolumeMusique()
        {
            string txtCompletVolumeMusique = "";
            txtCompletVolumeMusique = txtVolumeMusique + valVolumeMusique;
            return txtCompletVolumeMusique;
        }
        private string getTxtCompletPointsDeVie()
        {
            string txtCompletPointsDeVie = "";
            txtCompletPointsDeVie = stringPointsDeVie + valPointsDeVie;
            return txtCompletPointsDeVie;
        }
        private string getTxtCompletQteMinisDoritos()
        {
            string txtCompletQteMinisDoritos = "";
            txtCompletQteMinisDoritos = stringQteMinisDoritos + valQteMinisDoritos;
            return txtCompletQteMinisDoritos;
        }
        private string getTxtCompletQteSalsa()
        {
            string txtCompletQteSalsa = "";
            txtCompletQteSalsa = stringQteSalsa + valQteSalsa;
            return txtCompletQteSalsa;
        }
        #region GestionCollisions

        private void detectionCollisions()
        {
            IDictionary<CoteObjets, Vector2[]> listeDroitesCarre;
            //Vérification Doritos VS CaisseBois
            IDictionary<CoteObjets, Vector2[]> listeDroitesTriangle = doritos.getDroitesCotes();
            foreach (Carre2D caisse in caissesDeBois)
            {
                 listeDroitesCarre = caisse.getDroitesCotes();
                bool siCollisionDoritosCaisse = false;
                CoteObjets coteCollision = CoteObjets.NULL;

                foreach (KeyValuePair<CoteObjets, Vector2[]> droiteTriangle in listeDroitesTriangle)
                {
                    foreach (KeyValuePair<CoteObjets, Vector2[]> droiteCarre in listeDroitesCarre)
                    {
                        if (intersection(droiteTriangle.Value, droiteCarre.Value))
                        {
                            siCollisionDoritosCaisse = true;
                            coteCollision = droiteCarre.Key;
                        }
                    }
                }
                if (siCollisionDoritosCaisse)
                {
                    Console.WriteLine("Il y a eu collision sur le côté " + coteCollision.ToString());
                    audio.jouerSonOuch();
                    valPointsDeVie -= caisse.getDommage();
                    caisse.inverserDirection(coteCollision);
                }
            }

            if(caissesDeBois != null && caissesDeBois.Count > 1)
            {
                List<Carre2D> listeCaisses_ALPHA = new List<Carre2D>(caissesDeBois);
                List<Carre2D> listeCaisses_BRAVO = new List<Carre2D>(caissesDeBois);
                IDictionary<CoteObjets, Vector2[]> droitesCaisse_ALPHA;
                IDictionary<CoteObjets, Vector2[]> droitesCaisse_BRAVO;
                bool siCollisionCaisses = false;
                CoteObjets coteCollision_APLHA = CoteObjets.NULL;
                CoteObjets coteCollision_BRAVO = CoteObjets.NULL;

                foreach(Carre2D caisse_ALPHA in listeCaisses_ALPHA)
                {
                    droitesCaisse_ALPHA = caisse_ALPHA.getDroitesCotes();
                    listeCaisses_BRAVO.Remove(caisse_ALPHA);
                    foreach(Carre2D caisse_BRAVO in listeCaisses_BRAVO)
                    {
                        droitesCaisse_BRAVO = caisse_BRAVO.getDroitesCotes();
                        siCollisionCaisses = false;
                        foreach(KeyValuePair<CoteObjets, Vector2[]> droite_ALPHA in droitesCaisse_ALPHA)
                        {
                            foreach(KeyValuePair<CoteObjets, Vector2[]> droite_BRAVO in droitesCaisse_BRAVO)
                            {
                                if(intersection(droite_ALPHA.Value, droite_BRAVO.Value))
                                {
                                    siCollisionCaisses = true;
                                    coteCollision_APLHA = droite_ALPHA.Key;
                                    coteCollision_BRAVO = droite_BRAVO.Key;
                                }
                            }
                        }

                        if (siCollisionCaisses)
                        {
                            Console.WriteLine("Il y a eu collision sur le côté " + coteCollision_APLHA.ToString() + coteCollision_BRAVO.ToString());
                            caisse_ALPHA.inverserDirection(coteCollision_APLHA);
                            caisse_BRAVO.inverserDirection(coteCollision_BRAVO);
                        }
                    }
                }
            }


            if (minisDoritos != null && caissesDeBois != null && minisDoritos.Count > 0)
            {
                List<Projectile3p> listeMinisDoritos = new List<Projectile3p>(minisDoritos);
                List<Carre2D> listeCaissesDeBois = new List<Carre2D>(caissesDeBois);

                IDictionary<CoteObjets, Vector2[]> listeDroitesProjectiles;
                CoteObjets coteCollision = CoteObjets.NULL;
                foreach(Carre2D caisse in listeCaissesDeBois)
                {
                    listeDroitesCarre = caisse.getDroitesCotes();
                    foreach(Projectile3p miniDoritos in listeMinisDoritos)
                    {
                        listeDroitesProjectiles = miniDoritos.getDroitesCotes();
                        bool siCollisionProjectileCaisse = false;
                        foreach (Vector2[] droiteProjectile in listeDroitesProjectiles.Values)
                        {

                            foreach (KeyValuePair<CoteObjets, Vector2[]>droiteCarre in listeDroitesCarre)
                            {
                                
                                if (intersection(droiteProjectile, droiteCarre.Value))
                                {
                                    siCollisionProjectileCaisse = true;
                                    coteCollision = droiteCarre.Key;
                                    Console.WriteLine("Collision");
                                }
                            }
                        }
                        if(siCollisionProjectileCaisse)
                        {
                            audio.jouerSonDestructionCaisse();
                            minisDoritos.Remove(miniDoritos);
                            caissesDeBois.Remove(caisse);
                            if (caisse.getDommage() > 10)
                            {
                                Vector2[][] newPoints = caisse.getPointsPourPetitesCaisses();
                                Carre2D petiteCaisse_ALPHA = new Carre2D(10, newPoints[0][0], newPoints[0][1], newPoints[0][2], newPoints[0][3]);
                                Carre2D petiteCaisse_BRAVO = new Carre2D(10, newPoints[1][0], newPoints[1][1], newPoints[1][2], newPoints[1][3]);
                                Carre2D petiteCaisse_CHARLIE = new Carre2D(10, newPoints[2][0], newPoints[2][1], newPoints[2][2], newPoints[2][3]);
                                Carre2D petiteCaisse_DELTA = new Carre2D(10, newPoints[3][0], newPoints[3][1], newPoints[3][2], newPoints[3][3]);

                                caissesDeBois.Add(petiteCaisse_ALPHA);
                                caissesDeBois.Add(petiteCaisse_BRAVO);
                                caissesDeBois.Add(petiteCaisse_CHARLIE);
                                caissesDeBois.Add(petiteCaisse_DELTA);
                            }
                            else if (caissesDeBois.Count <= 0 && valPointsDeVie > 0)
                            {
                                isWinner = true;
                            }
                        }
                    }
                }
                //Si les projectiles sortent du cadre
                //Bugguy
                foreach (Projectile3p miniDoritos in listeMinisDoritos)
                {
                        if (miniDoritos.getPremierPoint().Y >= 105.0f
                            || miniDoritos.getPremierPoint().Y <= -150.0f
                            || miniDoritos.getPremierPoint().X >= 300.0f
                            || miniDoritos.getPremierPoint().X <= -300.0f)
                        {
                            minisDoritos.Remove(miniDoritos);
                        }

                }
            }

            //Console.WriteLine("*******************");
            //Console.WriteLine(" Liste des droites ");
            //Console.WriteLine("*******************");
            //Console.WriteLine("Triangles: ");
            //foreach (KeyValuePair<CoteObjets, Vector2[]> droite in listeDroitesTriangle)
            //{
            //    Console.Write(droite.Key + " : ");
            //    Vector2[] listePoints = droite.Value;
            //    Console.Write(listePoints[0] + " --- ");
            //    Console.WriteLine(listePoints[1]);
            //}
            //Console.WriteLine("");

            //Console.WriteLine("Carré: ");
            //foreach (KeyValuePair<CoteObjets, Vector2[]> droite in listeDroitesCarre)
            //{
            //    Console.Write(droite.Key + " : ");
            //    Vector2[] listePoints = droite.Value;
            //    Console.Write(listePoints[0] + " --- ");
            //    Console.WriteLine(listePoints[1]);
            //}
        }
        #region MethodesWeb
        private bool intersection(Vector2[] droiteTriangle, Vector2[] droiteCarre)
        {
            // **************************************************
            // Méthodes trouvées sur le web
            // Traduite en français pour aider à la compréhension
            // **************************************************

            /*
             * Une droite est représentée par cette équation : y = ax + b
             * où "a" représente la pente et "b" est le décalage de "y" à l'origine
             * 
             * NOTE : Une division par zéro a pour résultat l'INFINI.
             * Si une droite est verticale, l'équation pour trouver "a" retournera l'INFINI
             * */
            bool siIntersection = false;

            // Calculer les valeur "a" pour chacune de deux droites
            float a_Triangle = (droiteTriangle[1].Y - droiteTriangle[0].Y) / (droiteTriangle[1].X - droiteTriangle[0].X);
            float a_Carre = (droiteCarre[1].Y - droiteCarre[0].Y) / (droiteCarre[1].X - droiteCarre[0].X);

            // Calculer les valeur "b" pour chacune de deux droites
            float b_Triangle = droiteTriangle[1].Y - a_Triangle * droiteTriangle[1].X;
            float b_Carre = droiteCarre[1].Y - a_Carre * droiteCarre[1].X;

            // Calculer les valeurs "x" et "y" pour le point d'intersection des deux lignes
            float x = (b_Carre - b_Triangle) / (a_Triangle - a_Carre);
            float y = a_Triangle * x + b_Triangle;

            // **************
            // Début des test
            if (float.IsInfinity(a_Triangle) && float.IsInfinity(a_Carre))
            {
                // Si les deux valeurs "a" sont l'INFINI, alors on a deux droites verticales.
                // Si les deux droites partagent le même "x", alors on vérifie alors la valeur "y".
                siIntersection = (droiteTriangle[0].X == droiteCarre[0].X) && (
                                    (droiteCarre[0].Y <= droiteTriangle[0].Y && droiteTriangle[0].Y <= droiteCarre[1].Y)
                                    || (droiteCarre[0].Y <= droiteTriangle[1].Y && droiteTriangle[1].Y <= droiteCarre[1].Y)
                                    );
            }
            else if (float.IsInfinity(a_Triangle) && !float.IsInfinity(a_Carre))
            {
                // La droite du triangle EST vertical et celle de la caisse NE L'EST PAS
                x = droiteTriangle[0].X;
                y = b_Carre * x + b_Carre;

                siIntersection = (
                    ((droiteTriangle[0].Y <= droiteTriangle[1].Y && LTE(droiteTriangle[0].Y, y) && GTE(droiteTriangle[1].Y, y)) || (droiteTriangle[0].Y >= droiteTriangle[1].Y && GTE(droiteTriangle[0].Y, y) && LTE(droiteTriangle[1].Y, y))) &&
                    ((droiteCarre[0].X <= droiteCarre[1].X && LTE(droiteCarre[0].X, x) && GTE(droiteCarre[1].X, x)) || (droiteCarre[0].X >= droiteCarre[1].X && GTE(droiteCarre[0].X, x) && LTE(droiteCarre[1].X, x))) &&
                    ((droiteCarre[0].Y <= droiteCarre[1].Y && LTE(droiteCarre[0].Y, y) && GTE(droiteCarre[1].Y, y)) || (droiteCarre[0].Y >= droiteCarre[1].Y && GTE(droiteCarre[0].Y, y) && LTE(droiteCarre[1].Y, y)))
                    );
            }
            else if (!float.IsInfinity(a_Triangle) && float.IsInfinity(a_Carre))
            {
                // La droite du triangle N'EST PAS vertical et celle de la caisse L'EST
                x = droiteCarre[0].X;
                y = a_Triangle * x + b_Triangle;

                siIntersection = (
                    ((droiteTriangle[0].X <= droiteTriangle[1].X && LTE(droiteTriangle[0].X, x) && GTE(droiteTriangle[1].X, x)) || (droiteTriangle[0].X >= droiteTriangle[1].X && GTE(droiteTriangle[0].X, x) && LTE(droiteTriangle[1].X, x))) &&
                    ((droiteTriangle[0].Y <= droiteTriangle[1].Y && LTE(droiteTriangle[0].Y, y) && GTE(droiteTriangle[1].Y, y)) || (droiteTriangle[0].Y >= droiteTriangle[1].Y && GTE(droiteTriangle[0].Y, y) && LTE(droiteTriangle[1].Y, y))) &&
                    ((droiteCarre[0].Y <= droiteCarre[1].Y && LTE(droiteCarre[0].Y, y) && GTE(droiteCarre[1].Y, y)) || (droiteCarre[0].Y >= droiteCarre[1].Y && GTE(droiteCarre[0].Y, y) && LTE(droiteCarre[1].Y, y)))
                    );
            }

            // Finalement, vérifier si le point d'interception est à l'intérieur de tous les points
            if (!siIntersection)
            {
                siIntersection = (
                    ((droiteTriangle[0].X <= droiteTriangle[1].X && LTE(droiteTriangle[0].X, x) && GTE(droiteTriangle[1].X, x)) || (droiteTriangle[0].X >= droiteTriangle[1].X && GTE(droiteTriangle[0].X, x) && LTE(droiteTriangle[1].X, x))) &&
                    ((droiteTriangle[0].Y <= droiteTriangle[1].Y && LTE(droiteTriangle[0].Y, y) && GTE(droiteTriangle[1].Y, y)) || (droiteTriangle[0].Y >= droiteTriangle[1].Y && GTE(droiteTriangle[0].Y, y) && LTE(droiteTriangle[1].Y, y))) &&
                    ((droiteCarre[0].X <= droiteCarre[1].X && LTE(droiteCarre[0].X, x) && GTE(droiteCarre[1].X, x)) || (droiteCarre[0].X >= droiteCarre[1].X && GTE(droiteCarre[0].X, x) && LTE(droiteCarre[1].X, x))) &&
                    ((droiteCarre[0].Y <= droiteCarre[1].Y && LTE(droiteCarre[0].Y, y) && GTE(droiteCarre[1].Y, y)) || (droiteCarre[0].Y >= droiteCarre[1].Y && GTE(droiteCarre[0].Y, y) && LTE(droiteCarre[1].Y, y))));
            }

            return siIntersection;
        }

        // ****************************
        // Méthodes trouvées sur le web
        // Celles-ci permettent d'ajuster la précision lors des différentes comparaisons possibles.
        // ****************************

        /// <summary>
        /// Less Than or Equal: Tells you if the left value is less than or equal to the right value
        /// with floating point precision error taken into account.
        /// </summary>
        /// <param name="leftVal">The value on the left side of comparison operator</param>
        /// <param name="rightVal">The value on the right side of comparison operator</param>
        /// <returns>True if the left value and right value are within 0.000001 of each other, or if leftVal is less than rightVal</returns>
        private bool LTE(float leftVal, float rightVal)
        {
            return (EE(leftVal, rightVal) || leftVal < rightVal);

        }

        /// <summary>
        /// Greather Than or Equal: Tells you if the left value is greater than or equal to the right value
        /// with floating point precision error taken into account.
        /// </summary>
        /// <param name="leftVal">The value on the left side of comparison operator</param>
        /// <param name="rightVal">The value on the right side of comparison operator</param>
        /// <returns>True if the left value and right value are within 0.000001 of each other, or if leftVal is greater than rightVal</returns>
        private bool GTE(float leftVal, float rightVal)
        {
            return (EE(leftVal, rightVal) || leftVal > rightVal);
        }

        /// <summary>
        /// Equal-Equal: Tells you if two doubles are equivalent even with floating point precision errors
        /// </summary>
        /// <param name="Val1">First double value</param>
        /// <param name="Val2">Second double value</param>
        /// <returns>true if they are within 0.000001 of each other, false otherwise.</returns>
        private bool EE(float Val1, float Val2)
        {
            return (System.Math.Abs(Val1 - Val2) < 0.000001f);
        }

        /// <summary>
        /// Equal-Equal: Tells you if two doubles are equivalent even with floating point precision errors
        /// </summary>
        /// <param name="Val1">First double value</param>
        /// <param name="Val2">Second double value</param>
        /// <param name="Epsilon">The delta value the two doubles need to be within to be considered equal</param>
        /// <returns>true if they are within the epsilon value of each other, false otherwise.</returns>
        private bool EE(float Val1, float Val2, float Epsilon)
        {
            return (System.Math.Abs(Val1 - Val2) < Epsilon);
        }

        #endregion //MethodesWeb
        #endregion //GestionCollisions
    }
}
