using OpenTK.Audio.OpenAL;
using System;
using System.IO;

namespace PremierDessin__Heritage_
{
    internal class FichierWAV
    {
        // Attributs
        string nomFichier;
        int nbrCanaux;
        int frequence;
        int nbrBits;
        int qteDonneesSonores;
        byte[] donneesSonores;
        // Constructeur
        public FichierWAV(string nomFichier)
        {
            this.nomFichier = nomFichier;
            Stream fichierAudio = File.Open(nomFichier, FileMode.Open);
            chargerFichier(fichierAudio);
            fichierAudio.Close();
        }
        // Méthodes privées
        private void chargerFichier(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            int nbrOctets;
            string donneesFichier;
            BinaryReader reader = new BinaryReader(stream);
            // Lire la constante RIFF (identifie des fichiers multimédia)
            nbrOctets = 4;
            donneesFichier = new string(reader.ReadChars(nbrOctets));
            if (donneesFichier != "RIFF")
            {
                throw new NotSupportedException("N'est pas un fichier multimédia");
            }
            // Lire la taille du fichier
            donneesFichier = reader.ReadInt32().ToString();
            // Pas besoin de spécifier le nombre d'octets à lire,
            // car "ReadInt32()" lit précisément 4 octets
            // Lire le format (toujours 4 octets)
            donneesFichier = new string(reader.ReadChars(nbrOctets));
            if (donneesFichier != "WAVE")
            {
                throw new NotSupportedException("Le fichier audio n'est pas au format WAVE");
            }
            // Lire l'entête WAVE
            donneesFichier = new string(reader.ReadChars(nbrOctets));
            if (donneesFichier != "fmt ")
            {
                throw new NotSupportedException("Fichier WAVE non supporté (fmt).");
            }
            // Lire 6 octets que nous n'utiliserons pas
            // (permet simplement d'avancer la tête de lecture)
            nbrOctets = 6;
            donneesFichier = new string(reader.ReadChars(nbrOctets));
            // Lire le nombre de canaux (int16 = 2 octets)
            nbrCanaux = reader.ReadInt16();
            // Lire la fréquence (int32 = 4 octets)
            frequence = reader.ReadInt32();
            // Lire 6 octets que nous n'utiliserons pas
            // (permet simplement d'avancer la tête de lecture)
            nbrOctets = 6;
            donneesFichier = new string(reader.ReadChars(nbrOctets));
            // Lire le nombre de bits par échantillon (int16 = 2 octets)
            nbrBits = reader.ReadInt16();
            // S'assurer que nous sommes rendus à lire la section des données
            nbrOctets = 4;
            donneesFichier = new string(reader.ReadChars(nbrOctets));
            if (donneesFichier != "data")
            {
                throw new NotSupportedException("Fichier WAVE non supporté (data).");
            }
            // Lire la quantité de données sonores (int32 = 4 octets)
            qteDonneesSonores = reader.ReadInt32();
            // Lire les données sonores
            donneesSonores = reader.ReadBytes(qteDonneesSonores);
        }
        // Méthodes publiques
        public ALFormat getFormatSonAL()
        {
            ALFormat format;
            switch (nbrCanaux)
            {
                case 1: format = (nbrBits == 8 ? ALFormat.Mono8 : ALFormat.Mono16); break;
                case 2: format = (nbrBits == 8 ? ALFormat.Stereo8 : ALFormat.Stereo16); break;
                default: throw new NotSupportedException("Format non supporté.");
            }
            return format;
        }
        public byte[] getDonneesSonores()
        {
            return donneesSonores;
        }
        public int getQteDonneesSonores()
        {
            return qteDonneesSonores;
        }
        public int getFrequence()
        {
            return frequence;
        }
    }
}
