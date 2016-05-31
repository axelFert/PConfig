using PConfig.Tools.Mysql;
using System;
using System.Collections.Generic;
using System.Data;

namespace PConfig.Model.DAO
{
    public class PlaceDAO : SmgDao<Place>
    {
        /// <summary>
        /// LOGGER
        /// </summary>
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(PlaceDAO));

        #region singleton

        private static PlaceDAO Instance;
        private static readonly object instanceLock = new object();

        private PlaceDAO()
        {
        }

        public static PlaceDAO getInstance()
        {
            if (Instance == null)
            {
                lock (instanceLock)
                {
                    Instance = new PlaceDAO();
                }
            }
            return Instance;
        }

        #endregion singleton

        public List<Place> getAllPlaceByZone(int idZone)
        {
            string request = string.Format("Select * from tblmotes where type = 'sensor' and ID_zone = {0}", idZone);
            MySqlTools sql = MySqlTools.getConnection();
            List<Place> lstPlace = new List<Place>();

            DataTable data = sql.executeRequest(request);
            foreach (DataRow row in data.Rows)
            {
                Place place = Constructeur<Place>.createInstance(row);
                place.InitObj();
                lstPlace.Add(place);
            }

            return lstPlace;
        }

        public List<string> GetAllCategory()
        {
            string query = "SELECT DISTINCT(category) as categorie FROM tblmotes";
            MySqlTools sql = MySqlTools.getConnection();
            List<string> lstCat = new List<string>();

            DataTable data = sql.executeRequest(query);
            foreach (DataRow row in data.Rows)
            {
                if (row[0] != null)
                {
                    lstCat.Add(row[0].ToString());
                }
            }
            return lstCat;
        }

        public override List<Place> getAll()
        {
            string request = "Select m.*,p.id_panel as NumeroTotemRadio from tblmotes as m INNER JOIN db_smg_run.tblpanels AS p ON p.id_pan = m.id_pan AND p.ID_mac = (m.ID_mac & 0xff00) where type = 'sensor' ";
            MySqlTools sql = MySqlTools.getConnection();
            List<Place> lstPlace = new List<Place>();

            DataTable data = sql.executeRequest(request);

            foreach (DataRow row in data.Rows)
            {
                Place place = Constructeur<Place>.createInstance(row);
                place.InitObj();
                lstPlace.Add(place);
            }

            return lstPlace;
        }

        public override bool Insert(Place place)
        {
            throw new NotImplementedException();
        }

        public override bool Update(Place place)
        {
            string request = "UPDATE tblmotes SET name = '{0}' WHERE ID_pan = '{1}' and ID_mac = '{2}';";
            request = string.Format(request, place.name, place.ID_pan, place.ID_mac);
            MySqlTools sql = MySqlTools.getConnection();
            bool result = sql.executeUpdate(request);
            return result;
        }
    }
}