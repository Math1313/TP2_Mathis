using System;
using OpenTK;
using OpenTK.Graphics;

namespace BaseOpenTk
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region Attributs
            int largeurFenetre = 600;
            int hauteurFenetre = 300;
            string titreFenetre = "Jeu - Mouvements";
            #endregion //Attributs

            #region Code
            GameWindow window = new GameWindow(largeurFenetre, hauteurFenetre, GraphicsMode.Default, titreFenetre);
            GestionJeu fenetrePrincipale = new GestionJeu(window);
            #endregion //Code

        }
    }
}
