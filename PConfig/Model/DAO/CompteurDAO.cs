using PConfig.Tools.Mysql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PConfig.Model.DAO
{
    internal class CompteurDAO : SmgDao<Compteur>
    {
        #region singleton

        private static CompteurDAO Instance;
        private static readonly object instanceLock = new object();

        private CompteurDAO()
        {
        }

        public static CompteurDAO getInstance()
        {
            if (Instance == null)
            {
                lock (instanceLock)
                {
                    Instance = new CompteurDAO();
                }
            }
            return Instance;
        }

        #endregion singleton

        public override List<Compteur> getAll()
        {
            string request = "SELECT 	polecount.ID_pole_count AS Id, polecount.name AS Nom, mote.name AS NomPlace, CONCAT(sensor.ID_pan, '/', sensor.ID_mac) AS PanMacPlace" +
                " FROM tblpole_count_has_sensors AS sensor" +
                " INNER JOIN tblpole_counts AS polecount ON polecount.ID_pole_count = sensor.ID_pole_count" +
                " INNER JOIN tblmotes AS mote ON (sensor.ID_mac = mote.ID_mac AND mote.id_pan = sensor.ID_pan) ORDER BY PanMacPlace,Nom";

            MySqlTools sql = MySqlTools.getConnection();
            DataTable data = sql.executeRequest(request);
            Dictionary<string, Compteur> dico = new Dictionary<string, Compteur>();
            foreach (DataRow row in data.Rows)
            {
                Compteur compteur = Constructeur<Compteur>.createInstance(row);
                if (!dico.ContainsKey(compteur.Nom))
                {
                    compteur.LstPanMac = new List<string>();
                    compteur.LstPanMac.Add(compteur.PanMacPlace);
                    dico.Add(compteur.Nom, compteur);
                }
                else
                {
                    dico[compteur.Nom].LstPanMac.Add(compteur.PanMacPlace);
                }
            }
            return dico.Values.ToList(); ;
        }

        public override bool Insert(Compteur obj)
        {
            throw new NotImplementedException();
        }

        public override bool Update(Compteur obj)
        {
            throw new NotImplementedException();
        }
    }
}