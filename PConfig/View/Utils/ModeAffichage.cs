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

    internal class TypeDessin
    {
        private string NomMode;
        public TYPE_DESSIN Mode { get; }

        public TypeDessin(string nom, TYPE_DESSIN mode)
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