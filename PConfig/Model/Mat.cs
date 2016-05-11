using PConfig.Model.DAO;
using System;
using System.Collections.Generic;

namespace PConfig.Model
{
    public class Mat
    {
        public int Id { get; set; }

        public string Nom { get; set; }

        public string IdPan { get; set; }

        public string IdMac { get; set; }

        public int idCompteur { get; set; }

        public string Afficheur { get; set; }

        public List<Tuple<string, int>> AfficheursId { get; set; }

        public Dictionary<string, Compteur> Afficheurs { get; set; } = new Dictionary<string, Compteur>();
    }
}