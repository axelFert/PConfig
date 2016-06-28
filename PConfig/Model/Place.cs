using PConfig.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PConfig.Model
{
    /// <summary>
    /// Classe represantant une Place defini a partir de son coin superieur Gauche Data model
    /// </summary>
    public class Place : SmgObj, ILegendObject
    {
        #region Declaration

        /// <summary>
        /// LOGGER
        /// </summary>
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
        (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public double X { get; set; }

        public double Y { get; set; }

        public double Hauteur { get; set; }

        public double Longueur { get; set; }

        public int Numero { get; set; }

        public int Niveau { get; set; }

        public double Angle { get; set; }

        public Dictionary<int, int> LstTotemDispalyer { get; set; }

        public int NumeroTotemRadio { get; set; }

        #endregion Declaration

        /// <summary>
        /// juste un constructeur vide. la classe Constructeur se charge d'instancier la classe <see cref="Constructeur{T}"/>
        /// </summary>
        public Place()
        {
        }

        public override void InitObj()
        {
            calculSize();

            IdTotemRadio = int.Parse(ID_pan + "" + (ID_mac & SmgUtil.MASQUE_TOTEM_RADIO_MAC));
            Numero = (ID_mac & SmgUtil.MASQUE_NUMERO_PLACE_MAC);
            LstTotemDispalyer = new Dictionary<int, int>();
        }

        public override void calculSize()
        {
            string[] dim = this.polygon.Split(',');
            // le centre du rectangle
            double X = Double.Parse(dim[0].Split('.')[0]);
            double Y = Double.Parse(dim[1].Split('.')[0]);

            Longueur = Double.Parse(dim[2].Split('.')[0]);
            Hauteur = Double.Parse(dim[3].Split('.')[0]);
            Angle = Double.Parse(dim[4].Split('.')[0]);
            if (90 < Angle && Angle < 280)
            {
                Angle += 180;
            }

            this.X = X - Longueur / 2;
            this.Y = Y - Hauteur / 2;
        }

        public List<Propriete> GetInfo()
        {
            List<Propriete> lst = new List<Propriete>();
            lst.Add(new Propriete("Pan", ID_pan.ToString()));
            lst.Add(new Propriete("Mac", ID_mac.ToString()));
            lst.Add(new Propriete("Catégorie", category));
            lst.Add(new Propriete("Totem radio", NumeroTotemRadio.ToString()));
            lst.Add(new Propriete("Nombre Afficheur multipanel", LstTotemDispalyer.Count.ToString()));
            lst.Add(new Propriete("Afficheurs", string.Join(",", LstTotemDispalyer.Keys.ToArray())));
            lst.Add(new Propriete("Hub", NumeroHub.ToString()));

            return lst;
        }

        public string getNom()
        {
            return this.name;
        }

        public string getType()
        {
            return "Place";
        }
    }
}