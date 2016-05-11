using PConfig.View.Utils;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PConfig.View
{
    public abstract class SmgObjView : Shape, IEquatable<SmgObjView>
    {
        /// <summary>
        /// le texte affiche dans la place
        /// </summary>
        public Label text;

        /// <summary>
        /// boolean pour savoir si l'élément est séléctionner
        /// </summary>
        public bool isSelected { get; protected set; }

        /// <summary>
        /// le nom de l'objet (doit etre unique)
        /// </summary>
        public string NameObj { get; set; }

        /// <summary>
        /// le totem auquel est relié le grain en radio pan mac plus masque
        /// </summary>
        public int TotemRadio { get; set; }

        /// <summary>
        /// pas utilisé.
        /// </summary>
        public int TotemComptage { get; set; }

        /// <summary>
        /// le id panel de l'objet
        /// </summary>
        public int IdPanel { get; set; }

        public ETAT_OBJET_PLAN Etat { get; set; }

        /// <summary>
        /// pan de l'objet
        /// </summary>
        public int Pan { get; set; }

        /// <summary>
        /// mac de l'objet
        /// </summary>
        public int Mac { get; set; }

        protected SmgObjView()
        {
            text = new Label();
            MouseLeftButtonDown += SelectObject;
            text.MouseLeftButtonDown += SelectObject;
            isSelected = false;
            //Etat = ETAT_OBJET_PLAN.NONE;
        }

        /// <summary>
        /// mise a jour de l'affichage de l'objet graphique
        /// </summary>
        /// <param name="newMode"></param>
        public abstract void updateAffichageProp(MODE_AFFICHAGE_OBJET newMode);

        protected abstract void SelectObject(object sender, RoutedEventArgs e);

        public abstract void UpdateState(SmgObjView sender, Boolean multiSelect);

        public bool Equals(SmgObjView other)
        {
            return (this.Pan == other.Pan && this.Mac == other.Mac);
        }

        public static bool operator ==(SmgObjView a, SmgObjView b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.Equals(b);
        }

        public static bool operator !=(SmgObjView a, SmgObjView b)
        {
            return !(a == b);
        }

        public abstract void UpdateColor();
    }
}