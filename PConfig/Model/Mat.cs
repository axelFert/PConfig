using PConfig.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PConfig.Model
{
    public class Mat : SmgObj, ILegendObject
    {
        public double XCentre { get; set; }
        public double YCentre { get; set; }
        public double TailleCote { get; set; }

        public int Id { get; set; }

        //public string Nom { get; set; }

        public int idCompteur { get; set; }

        public string Afficheur { get; set; }

        public List<Tuple<string, int>> AfficheursId { get; set; }

        public Dictionary<string, Compteur> Afficheurs { get; set; } = new Dictionary<string, Compteur>();

        public override void InitObj()
        {
            calculSize();
            IdTotemRadio = int.Parse(ID_pan + "" + (ID_mac & SmgUtil.MASQUE_TOTEM_RADIO_MAC));
        }

        public override void calculSize()
        {
            string[] dim = this.polygon.Split(',');
            XCentre = Double.Parse(dim[0]);
            YCentre = Double.Parse(dim[1]);
            TailleCote = 15;
        }

        public List<Propriete> GetInfo()
        {
            List<Propriete> lst = new List<Propriete>();
            lst.Add(new Propriete("Id Mat", ID_panels.ToString()));
            lst.Add(new Propriete("Pan", ID_pan.ToString()));
            lst.Add(new Propriete("Mac", ID_mac.ToString()));
            Afficheurs.Keys.ToList().ForEach(ob => lst.Add(new Propriete(ob + "-" + Afficheurs[ob].Nom, Afficheurs[ob].PlaceComptees.Count().ToString())));

            return lst;
        }

        public string getNom()
        {
            return this.name;
        }

        public string getType()
        {
            return "Mat";
        }
    }
}