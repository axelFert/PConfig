using PConfig.Model;
using PConfig.View.Utils;

namespace PConfig.View
{
    public interface IAbstractNiveau
    {
        void UpdateAffichage(MODE_AFFICHAGE_OBJET newMode);

        void UpdateAffichageTotem(MODE_AFFICHAGE_OBJET newMode);

        /// <summary>
        /// selection d'un compteur dans le tree
        /// </summary>
        /// <param name="cpt"></param>
        void updateViewCompteur(Compteur cpt);

        /// <summary>
        /// fonction lorsqu'un element est selectionné dans le tree
        /// </summary>
        /// <param name="pan"></param>
        /// <param name="mac"></param>
        void SelectionSmgObj(int pan, int mac);

        /// <summary>
        /// Selection d'un hub dans le menu
        /// </summary>
        /// <param name="hub"></param>
        void SelectionSmgObjByHub(int Hub);

        /// <summary>
        /// mise a jour des couleur des place
        /// </summary>
        void UpdateColor();

        /// <summary>
        /// selection de toutes les places de ce type
        /// </summary>
        /// <param name="type"></param>
        void SelectionTypePlace(string type);
    }
}