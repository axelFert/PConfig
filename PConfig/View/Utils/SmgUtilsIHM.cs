using System.Collections.Generic;
using static PConfig.Tools.SmgUtil;

namespace PConfig.View.Utils
{
    public static class SmgUtilsIHM
    {
        public static Dictionary<ETAT_OBJET_PLAN, ColorState> LIST_COULEUR_ETAT;

        public static Dictionary<TYPE_PLACE, ColorState> LIST_COULEUR_TYPE;

        public static ColorState COULEUR_PLACE_SELECTION;

        public static ColorState getColorEtat(ETAT_OBJET_PLAN etat)
        {
            return LIST_COULEUR_ETAT[etat];
        }

        public static int EPAISSEUR_TRAIT = 1;
        public static int TAILLE_POLICE = 1;

        public static bool IS_NEGATIF = false;

        public static bool IS_RADIO_LINK = false;

        public static int DIAMETRE_TOTEM = 20;
        public static int COTE_MAT = 020;

        public static bool TAILLE_POLICE_AUTO = true;
    }

    /// <summary>
    /// Enum qui va deffinir les différent etat des palce sur le plan si elle sont surligné pour
    /// comptage, pour multipanel ou autre
    /// </summary>
    public enum ETAT_OBJET_PLAN
    {
        NONE_PLACE, NONE_TOTEM, CONNEXION_RADIO, COMPTAGE_RADIO, COMPTAGE_MULTIPANEL, COMPTAGE_COMPLEX, ELEMENT_LIE,
        NONE_MAT
    }

    public enum MODE_AFFICHAGE_OBJET
    {
        NOM, PAN_MAC, ID, NOMBRE_PLACE, TYPE
    }

    public enum TYPE_DESSIN
    {
        PLACE, TOTEM, MAT, LIBRE
    }

    public enum ETAT_PLAN
    {
        VERIFICATION, CREATION, MODIFICATION
    }
}