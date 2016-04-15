using PConfig.Tools.Mysql;
using System;
using System.Collections.Generic;
using System.Data;

namespace PConfig.Model.DAO
{
    public class TotemDAO : SmgDao<Totem>
    {
        /// <summary>
        /// LOGGER
        /// </summary>
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(PlaceDAO));

        #region singleton work

        private static TotemDAO Instance;
        private static readonly object instanceLock = new object();

        private TotemDAO()
        {
        }

        public static TotemDAO getInstance()
        {
            if (Instance == null)
            {
                lock (instanceLock)
                {
                    Instance = new TotemDAO();
                }
            }
            return Instance;
        }

        #endregion singleton work

        public override List<Totem> getAll()
        {
            string request = string.Format("Select * from tblmotes where type = 'panel' order by id_panels");
            MySqlTools sql = MySqlTools.getConnection();
            List<Totem> lstTotem = new List<Totem>();

            DataTable data = sql.executeRequest(request);
            foreach (DataRow row in data.Rows)
            {
                Totem totem = Constructeur<Totem>.createInstance(row);
                totem.InitObj();
                lstTotem.Add(totem);
            }
            return lstTotem;
        }

        public List<Totem> getAllTotemByZone(int idZone)
        {
            string request = string.Format("Select * from tblmotes where type = 'panel' and ID_zone = {0}", idZone);
            MySqlTools sql = MySqlTools.getConnection();
            List<Totem> lstTotem = new List<Totem>();

            DataTable data = sql.executeRequest(request);
            foreach (DataRow row in data.Rows)
            {
                Totem totem = Constructeur<Totem>.createInstance(row);
                totem.InitObj();
                lstTotem.Add(totem);
            }

            request = string.Format("SELECT DISTINCT ID_panel_displayer, ID_panel_counter,"
                + "GROUP_CONCAT(CONCAT(ID_panel_counter,'/',counter_mask) SEPARATOR ',') AS counterMask"
                + "FROM tblpanels_multipanel GROUP BY ID_panel_displayer; ");
            return lstTotem;
        }

        public override bool Insert(Totem obj)
        {
            throw new NotImplementedException();
        }

        public override bool Update(Totem obj)
        {
            throw new NotImplementedException();
        }
    }
}