using System;

namespace PConfig.View.Utils
{
    internal class ModeAffichage
    {
        private string NomMode;
        public MODE_AFFICHAGE_OBJET Mode { get; }

        public ModeAffichage(string nom, MODE_AFFICHAGE_OBJET mode)
        {
            NomMode = nom;
            Mode = mode;
        }

        public override string ToString()
        {
            return NomMode;
        }
    }
}