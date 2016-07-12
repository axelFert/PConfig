using PConfig.Tools;

namespace PConfig.Model
{
    /// <summary>
    /// Class abstraite SMG represenatant les motes/totem depuis la table db_smg_run tblmotes
    /// </summary>
    public abstract class SmgObj
    {
        /// <summary>
        /// ID pan
        /// </summary>
        public int ID_pan { get; set; }

        /// <summary>
        /// id mac
        /// </summary>
        public int ID_mac { get; set; }

        /// <summary>
        /// nom de l'objet ex 1.1.131
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// l'idd des panels
        /// </summary>
        public int ID_panels { get; set; }

        /// <summary>
        /// </summary>
        public int master { get; set; }

        public int ID_serial { get; set; }
        public int ID_network { get; set; }
        public string category { get; set; }
        public int ID_zone { get; set; }
        public string polygon { get; set; }

        /// <summary>
        /// Recuperation du numéro de hub la double division par 16 permet de récuperer le numero (1
        /// 2 3 4) le -1 car les hub commencent a 0
        /// </summary>
        public int NumeroHub { get { return ((ID_pan & SmgUtil.MASQUE_HUB_PAN) / 16 / 16) - 1; } }

        public int Frequence { get { return (ID_pan & SmgUtil.MASQUE_FREQUENCE); } }

        /// <summary> Id auqel est relie l'objet en radio (pan + (mac & 0xff00) ) </summary>
        public int IdTotemRadio { get; set; }

        /// <summary>
        /// methode pour calculer la taille de l'objet avec le polygon
        /// </summary>
        public abstract void calculSize();

        public abstract void InitObj();

        public override bool Equals(object obj)
        {
            var item = obj as SmgObj;

            if (item == null)
            {
                return false;
            }

            return this.name.Equals(item.name);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return name;
        }

        public bool Equals(SmgObj other)
        {
            return (this.ID_pan == other.ID_pan && this.ID_mac == other.ID_mac);
        }

        //add this code to class ThreeDPoint as defined previously
        //
        public static bool operator ==(SmgObj a, SmgObj b)
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

        public static bool operator !=(SmgObj a, SmgObj b)
        {
            return !(a == b);
        }
    }
}