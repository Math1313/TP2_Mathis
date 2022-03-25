using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;

namespace PremierDessin__Heritage_
{
    internal class GestionAudio
    {
        #region Attributs
        AudioContext audioContext;
        int bufferMusique;
        int sourceMusique;
        FichierWAV fichierMusiquePrincipale;
        float volumeMusique;

        //OUCH
        int bufferOuch;
        int sourceOuch;
        FichierWAV fichierOuch;

        //PEWPEW
        int bufferPewpew;
        int sourcePewpew;
        FichierWAV fichierPewpew;

        //SALSA
        int bufferSalsa;
        int sourceSalsa;
        FichierWAV fichierSalsa;

        //SPLASH
        int bufferSplash;
        int sourceSplash;
        FichierWAV fichierSplash;

        //PLEURES
        int bufferPleures;
        int sourcePleures;
        FichierWAV fichierPleures;

        //DESTRUCTION CAISSE
        int bufferDestructionCaisse;
        int sourceDestructionCaisse;
        FichierWAV fichierDestructionCaisse;

        //WINNER
        int bufferWinner;
        int sourceWinner;
        FichierWAV fichierWinner;

        //DORITOS
        int bufferDoritos;
        int sourceDoritos;
        FichierWAV fichierDoritos;

        //APPLAUSE
        int bufferApplause;
        int sourceApplause;
        FichierWAV fichierApplause;

        #endregion // Attributs

        #region ConstructeurInitialisateurFinalizer
        public GestionAudio()
        {
            audioContext = new AudioContext();
            fichierMusiquePrincipale = new FichierWAV("./audio/DarkAtmosphere.wav");
            volumeMusique = 1.0f;
            fichierOuch = new FichierWAV("./audio/Ouch.wav");
            fichierPewpew = new FichierWAV("./audio/PewPew.wav");
            fichierSalsa = new FichierWAV("./audio/Salsa.wav");
            fichierSplash = new FichierWAV("./audio/Splash.wav");
            fichierPleures = new FichierWAV("./audio/Pleures.wav");
            fichierDestructionCaisse = new FichierWAV("./audio/DestructionCaisse.wav");
            //fichierDoritos = new FichierWAV("./audio/Doritos.wav");
            //fichierApplause = new FichierWAV("./audio/Applause.wav");
            init();
        }

        private void init()
        {
            bufferMusique = AL.GenBuffer();
            sourceMusique = AL.GenSource();
            AL.BufferData(bufferMusique, fichierMusiquePrincipale.getFormatSonAL(), fichierMusiquePrincipale.getDonneesSonores(),
                fichierMusiquePrincipale.getQteDonneesSonores(), fichierMusiquePrincipale.getFrequence());
            AL.Source(sourceMusique, ALSourcei.Buffer, bufferMusique);
            AL.Source(sourceMusique, ALSourceb.Looping, true);

            //OUCH
            bufferOuch = AL.GenBuffer();
            sourceOuch = AL.GenSource();
            AL.BufferData(bufferOuch, fichierOuch.getFormatSonAL(), fichierOuch.getDonneesSonores(),
                fichierOuch.getQteDonneesSonores(), fichierOuch.getFrequence());
            AL.Source(sourceOuch, ALSourcei.Buffer, bufferOuch);
            AL.Source(sourceOuch, ALSourceb.Looping, false);

            //PEWPEW
            bufferPewpew = AL.GenBuffer();
            sourcePewpew = AL.GenSource();
            AL.BufferData(bufferPewpew, fichierPewpew.getFormatSonAL(), fichierPewpew.getDonneesSonores(),
                fichierPewpew.getQteDonneesSonores(), fichierPewpew.getFrequence());
            AL.Source(sourcePewpew, ALSourcei.Buffer, bufferPewpew);
            AL.Source(sourcePewpew, ALSourceb.Looping, false);

            //SALSA
            bufferSalsa = AL.GenBuffer();
            sourceSalsa = AL.GenSource();
            AL.BufferData(bufferSalsa, fichierSalsa.getFormatSonAL(), fichierSalsa.getDonneesSonores(),
                fichierSalsa.getQteDonneesSonores(), fichierSalsa.getFrequence());
            AL.Source(sourceSalsa, ALSourcei.Buffer, bufferSalsa);
            AL.Source(sourceSalsa, ALSourceb.Looping, false);

            //SPLASH
            bufferSplash = AL.GenBuffer();
            sourceSplash = AL.GenSource();
            AL.BufferData(bufferSplash, fichierSplash.getFormatSonAL(), fichierSplash.getDonneesSonores(),
                fichierSplash.getQteDonneesSonores(), fichierSplash.getFrequence());
            AL.Source(sourceSplash, ALSourcei.Buffer, bufferSplash);
            AL.Source(sourceSplash, ALSourceb.Looping, false);

            //PLEURES
            bufferPleures = AL.GenBuffer();
            sourcePleures = AL.GenSource();
            AL.BufferData(bufferPleures, fichierPleures.getFormatSonAL(), fichierPleures.getDonneesSonores(),
                fichierPleures.getQteDonneesSonores(), fichierPleures.getFrequence());
            AL.Source(sourcePleures, ALSourcei.Buffer, bufferPleures);
            AL.Source(sourcePleures, ALSourceb.Looping, false);

            //DESTRUCTION CAISSE
            bufferDestructionCaisse = AL.GenBuffer();
            sourceDestructionCaisse = AL.GenSource();
            AL.BufferData(bufferDestructionCaisse, fichierDestructionCaisse.getFormatSonAL(), fichierDestructionCaisse.getDonneesSonores(),
                fichierDestructionCaisse.getQteDonneesSonores(), fichierDestructionCaisse.getFrequence());
            AL.Source(sourceDestructionCaisse, ALSourcei.Buffer, bufferDestructionCaisse);
            AL.Source(sourceDestructionCaisse, ALSourceb.Looping, false);

            ////DORITOS
            //bufferDoritos = AL.GenBuffer();
            //sourceDoritos = AL.GenSource();
            //AL.BufferData(bufferDoritos, fichierDoritos.getFormatSonAL(), fichierDoritos.getDonneesSonores(),
            //    fichierDoritos.getQteDonneesSonores(), fichierDoritos.getFrequence());
            //AL.Source(sourceDoritos, ALSourcei.Buffer, bufferDoritos);
            //AL.Source(sourceDoritos, ALSourceb.Looping, false);

            ////APPLAUSE
            //bufferApplause = AL.GenBuffer();
            //sourceApplause = AL.GenSource();
            //AL.BufferData(bufferApplause, fichierApplause.getFormatSonAL(), fichierApplause.getDonneesSonores(),
            //    fichierApplause.getQteDonneesSonores(), fichierApplause.getFrequence());
            //AL.Source(sourceApplause, ALSourcei.Buffer, bufferApplause);
            //AL.Source(sourceApplause, ALSourceb.Looping, false);


            //Mettre le bon niveau sonore
            AL.Listener(ALListenerf.Gain, volumeMusique);
        }
        ~GestionAudio()
        {
            AL.SourceStop(sourceMusique);
            AL.DeleteSource(sourceMusique);
            AL.DeleteBuffer(bufferMusique);
        }
        #endregion

        public void demarrerMusiqueDeFond()
        {
            AL.SourcePlay(sourceMusique);
        }

        public void jouerSonOuch()
        {
            AL.SourcePlay(sourceOuch);
        }

        public void jouerSonPewpew()
        {
            AL.SourcePlay(sourcePewpew);
        }
        public void jouerSonSalsa()
        {
            AL.SourcePlay(sourceSalsa);
        }
        public void jouerSonSplash()
        {
            AL.SourcePlay(sourceSplash);
        }
        public void jouerSonPleures()
        {
            AL.SourcePlay(sourcePleures);
        }

        public void jouerSonDestructionCaisse()
        {
            AL.SourcePlay(sourceDestructionCaisse);
        }
        //public void jouerDoritos()
        //{
        //    AL.SourcePlay(sourceDoritos);
        //}
        //public void jouerApplause()
        //{
        //    AL.SourcePlay(sourceApplause);
        //}
        public void jouerDefaite()
        {
            int bufferDefaite;
            int sourceDefaite;
            FichierWAV fichierDefaite = new FichierWAV("./audio/FailWahWah.wav");

            bufferDefaite = AL.GenBuffer();
            sourceDefaite = AL.GenSource();
            AL.BufferData(bufferDefaite, fichierDefaite.getFormatSonAL(), fichierDefaite.getDonneesSonores(),
                fichierDefaite.getQteDonneesSonores(), fichierDefaite.getFrequence());
            AL.Source(sourceDefaite, ALSourcei.Buffer, bufferDefaite);
            AL.Source(sourceDefaite, ALSourceb.Looping, false);

            AL.SourcePlay(sourceDefaite);

            //Attendre la fin du son avant de poursuivre et diminuer le volume
            float volumeMusique;
            AL.GetSource((uint)sourceMusique, ALSourcef.Gain, out volumeMusique);
            ALSourceState etatDefaite;
            do {
                if (volumeMusique > 0.0f)
                {
                    volumeMusique -= 0.000001f;
                    //Ajuster le volume avec la nouvelle valeur
                    AL.Source(sourceMusique, ALSourcef.Gain, volumeMusique);
                }
                etatDefaite = AL.GetSourceState(sourceDefaite);
            } while(etatDefaite == ALSourceState.Playing);

            AL.DeleteSource(sourceDefaite);
            AL.DeleteBuffer(bufferDefaite);
        }

        public void jouerGameover()
        {
            int bufferGameover;
            int sourceGameover;
            FichierWAV fichierGameover = new FichierWAV("./audio/GameOver.wav");

            bufferGameover = AL.GenBuffer();
            sourceGameover = AL.GenSource();
            AL.BufferData(bufferGameover, fichierGameover.getFormatSonAL(), fichierGameover.getDonneesSonores(),
                fichierGameover.getQteDonneesSonores(), fichierGameover.getFrequence());
            AL.Source(sourceGameover, ALSourcei.Buffer, bufferGameover);
            AL.Source(sourceGameover, ALSourceb.Looping, false);

            AL.SourcePlay(sourceGameover);

            ALSourceState etatGameover;
            do
            {
                etatGameover = AL.GetSourceState(sourceGameover);
            } while (etatGameover == ALSourceState.Playing);

            AL.DeleteSource(sourceGameover);
            AL.DeleteBuffer(bufferGameover);
        }

        public void jouerWinner()
        {
            int bufferWinner;
            int sourceWinner;
            FichierWAV fichierWinner = new FichierWAV("./audio/AndTheWinnerIs.wav");

            bufferWinner = AL.GenBuffer();
            sourceWinner = AL.GenSource();
            AL.BufferData(bufferWinner, fichierWinner.getFormatSonAL(), fichierWinner.getDonneesSonores(),
                fichierWinner.getQteDonneesSonores(), fichierWinner.getFrequence());
            AL.Source(sourceWinner, ALSourcei.Buffer, bufferWinner);
            AL.Source(sourceWinner, ALSourceb.Looping, false);

            AL.SourcePlay(sourceWinner);

            float volumeMusique;
            AL.GetSource((uint)sourceMusique, ALSourcef.Gain, out volumeMusique);
            ALSourceState etatWinner;
            do
            {
                if (volumeMusique > 0.0f)
                {
                    volumeMusique -= 0.000001f;
                    //Ajuster le volume avec la nouvelle valeur
                    AL.Source(sourceMusique, ALSourcef.Gain, volumeMusique);
                }
                etatWinner = AL.GetSourceState(sourceWinner);
            } while (etatWinner == ALSourceState.Playing);

            AL.DeleteSource(sourceWinner);
            AL.DeleteBuffer(sourceWinner);
        }
        public void jouerDoritos()
        {
            int bufferDoritos;
            int sourceDoritos;
            FichierWAV fichierDortios = new FichierWAV("./audio/Doritos.wav");

            bufferDoritos = AL.GenBuffer();
            sourceDoritos = AL.GenSource();
            AL.BufferData(bufferDoritos, fichierDortios.getFormatSonAL(), fichierDortios.getDonneesSonores(),
                fichierDortios.getQteDonneesSonores(), fichierDortios.getFrequence());
            AL.Source(sourceDoritos, ALSourcei.Buffer, bufferDoritos);
            AL.Source(sourceDoritos, ALSourceb.Looping, false);

            AL.SourcePlay(sourceDoritos);

            ALSourceState etatDoritos;
            do
            {
                etatDoritos = AL.GetSourceState(sourceDoritos);
            } while (etatDoritos == ALSourceState.Playing);

            AL.DeleteSource(sourceDoritos);
            AL.DeleteBuffer(sourceDoritos);
        }
        public void jouerApplause()
        {
            int bufferApplause;
            int sourceApplause;
            FichierWAV fichierApplause = new FichierWAV("./audio/Applause.wav");

            bufferApplause = AL.GenBuffer();
            sourceApplause = AL.GenSource();
            AL.BufferData(bufferApplause, fichierApplause.getFormatSonAL(), fichierApplause.getDonneesSonores(),
                fichierApplause.getQteDonneesSonores(), fichierApplause.getFrequence());
            AL.Source(sourceApplause, ALSourcei.Buffer, bufferApplause);
            AL.Source(sourceApplause, ALSourceb.Looping, false);

            AL.SourcePlay(sourceApplause);

            ALSourceState etatApplause;
            do
            {
                etatApplause = AL.GetSourceState(sourceApplause);
            } while (etatApplause == ALSourceState.Playing);

            AL.DeleteSource(sourceApplause);
            AL.DeleteBuffer(sourceApplause);
        }

        public void setVolumeMusique(int nouveauVolume)
        {
            volumeMusique = (float)nouveauVolume / 100;
            AL.Listener(ALListenerf.Gain, volumeMusique);
        }

        public bool effetSonoreEstEnTrainDeJouer()
        {
            bool estEnTrainDeJouer = false;
            ALSourceState etatPewPew;
            ALSourceState etatSalsa;
            ALSourceState etatSplash;
            ALSourceState etatPleures;

            etatPewPew = AL.GetSourceState(sourcePewpew);
            etatSalsa = AL.GetSourceState(sourceSalsa);
            etatSplash = AL.GetSourceState(sourceSplash);
            etatPleures = AL.GetSourceState(sourcePleures);

            if (etatPewPew == ALSourceState.Playing || etatSalsa == ALSourceState.Playing ||
                etatSplash == ALSourceState.Playing || etatPleures == ALSourceState.Playing)
            {
                estEnTrainDeJouer = true;
            }

            return estEnTrainDeJouer;
        }
    }
}
