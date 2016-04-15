using PConfig.Tools.Mysql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PConfig.Model.DAO
{
    public class MatDAO : SmgDao<Mat>
    {
        #region singleton

        private MatDAO()
        {
        }

        static MatDAO()
        {
        }

        private static MatDAO _instance = new MatDAO();
        public static MatDAO Instance { get { return _instance; } }

        #endregion singleton

        public override List<Mat> getAll()
        {
            string query =
                "SELECT counter.ID_pole_count AS IdCompteur,counter.name AS NomCompteur,disp.name AS Afficheur,motes.name AS Nom,motes.id_pan AS IdPan,motes.id_mac AS IdMac, pole.ID_pole AS Id " +
                "FROM tblpole_displays AS disp " +
                "INNER JOIN tblpole_counts AS counter ON(disp.ID_pole_count = counter.ID_pole_count)  " +
                "INNER JOIN tblpoles AS pole ON (disp.id_pole = pole.id_pole) " +
                "INNER JOIN tblmotes AS motes ON(pole.ID_mac = motes.ID_mac AND pole.ID_pan = motes.id_pan)";

            MySqlTools sql = MySqlTools.getConnection();
            DataTable data = sql.executeRequest(query);
            Dictionary<int, Mat> dico = new Dictionary<int, Mat>();
            foreach (DataRow row in data.Rows)
            {
                Mat panel = Constructeur<Mat>.createInstance(row);
                if (!dico.ContainsKey(panel.Id))
                {
                    panel.AfficheursId = new List<Tuple<string, int>>();
                    panel.AfficheursId.Add(new Tuple<string, int>(panel.Afficheur, panel.idCompteur));
                    dico.Add(panel.Id, panel);
                }
                else
                {
                    dico[panel.Id].AfficheursId.Add(new Tuple<string, int>(panel.Afficheur, panel.idCompteur));
                }
            }
            return dico.Values.ToList(); ;
        }

        public override bool Insert(Mat obj)
        {
            throw new NotImplementedException();
        }

        public override bool Update(Mat obj)
        {
            throw new NotImplementedException();
        }
    }
}