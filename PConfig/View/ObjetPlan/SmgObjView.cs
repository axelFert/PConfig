using PConfig.Tools;
using PConfig.View.Utils;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace PConfig.View.ObjetPlan
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
        /// Recuperation du numéro de hub la double division par 16 permet de récuperer le numero (1
        /// 2 3 4) le -1 car les hub commencent a 0
        /// </summary>
        public int NumeroHub { get { return ((Pan & SmgUtil.MASQUE_HUB_PAN) / 16 / 16) - 1; } }

        public int Frequence { get { return (Pan & SmgUtil.MASQUE_FREQUENCE); } }

        /// <summary>
        /// mac de l'objet
        /// </summary>
        public int Mac { get; set; }

        public event EventHandler SelectionObj;

        protected SmgObjView()
        {
            text = new Label();
            text.Padding = new Thickness(1);
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

        protected void SelectObject(object sender, RoutedEventArgs e)
        {
            isSelected = !isSelected;
            if (SelectionObj != null)
            {
                SelectionObj(this, new EventArgs());
            }
        }

        public abstract void ObjSelect(bool select);

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