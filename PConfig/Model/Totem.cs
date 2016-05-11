using PConfig.Tools;
using System;
using System.Collections.Generic;

namespace PConfig.Model
{
    /// <summary>
    /// Classe représentant un totem
    /// </summary>
    public class Totem : SmgObj, ILegendObject
    {
        public double Diametre { get; set; }
        public double XCentre { get; set; }
        public double YCentre { get; set; }

        public List<Place> PlaceRadio { get; set; }

        public List<Place> PlaceAffiche { get; set; }

        /// <summary>
        /// juste un constructeur vide. la classe Constructeur se charge d'instancier la classe
        /// </summary>
        public Totem()
        {
        }

        public override void InitObj()
        {
            calculSize();
            PlaceRadio = new List<Place>();
            PlaceAffiche = new List<Place>();
            string totem = ID_pan + "" + (ID_mac & SmgUtil.MASQUE_TOTEM_RADIO_MAC);
            IdTotemRadio = int.Parse(totem);
        }

        public override void calculSize()
        {
            string[] dim = this.polygon.Split(',');
            XCentre = Double.Parse(dim[0]);
            YCentre = Double.Parse(dim[1]);
            Diametre = double.Parse(dim[2]);
        }

        public List<Propriete> GetInfo()
        {
            List<Propriete> lst = new List<Propriete>();
            lst.Add(new Propriete("Id totem", ID_panels.ToString()));
            lst.Add(new Propriete("Pan", ID_pan.ToString()));
            lst.Add(new Propriete("Mac", ID_mac.ToString()));
            lst.Add(new Propriete("nombre place radio", PlaceRadio.Count.ToString()));
            lst.Add(new Propriete("nombre place comptées", PlaceAffiche.Count.ToString()));

            return lst;
        }

        public string getNom()
        {
            return this.name;
        }

        public string getType()
        {
            return "Totem";
        }
    }
}