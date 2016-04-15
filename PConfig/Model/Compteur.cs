using System;
using System.Collections.Generic;

namespace PConfig.Model.DAO
{
    public class Compteur : ILegendObject
    {
        public int Id { get; set; }

        public string Nom { get; private set; }

        public string NomTotem { get; set; }

        public string PanTotem { get; set; }

        public string MacTotem { get; set; }

        public string NomAfficheur { get; set; }

        public string PanMacPlace { get; set; }

        public List<string> LstPanMac { get; set; }

        public List<Place> PlaceComptees { get; set; }

        public List<Propriete> GetInfo()
        {
            List<Propriete> lst = new List<Propriete>();
            lst.Add(new Propriete("Id", Id.ToString()));
            lst.Add(new Propriete("Nombre places", PlaceComptees.Count.ToString()));

            return lst;
        }

        public string getNom()
        {
            return this.Nom;
        }

        public string getType()
        {
            return "Compteur";
        }
    }
}