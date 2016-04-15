using System.Windows.Media;

namespace PConfig.View.Utils
{
    /// <summary>
    /// Classe qui fait la liaison entre un etat de place et une couleur
    /// </summary>

    public class ColorState
    {
        public ColorState(ETAT_OBJET_PLAN etat, Color fill, Color border)
        {
            Etat = etat;
            CouleurRemplissage = fill;
            CouleurBordure = border;
        }

        public ColorState(Color fill, Color border)
        {
            Etat = 0;
            CouleurRemplissage = fill;
            CouleurBordure = border;
        }

        public ETAT_OBJET_PLAN Etat { get; set; }
        public Color CouleurRemplissage { get; set; }
        public Color CouleurBordure { get; set; }

        public override bool Equals(object obj)
        {
            ColorState other = obj as ColorState;
            if (other == null)
                return false;
            return Etat.Equals(other.Etat);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}