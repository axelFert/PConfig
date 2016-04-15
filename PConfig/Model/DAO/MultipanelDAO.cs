using PConfig.Tools.Mysql;
using System;
using System.Collections.Generic;
using System.Data;

namespace PConfig.Model.DAO
{
    public class MultipanelDAO : SmgDao<Multipanel>
    {
        #region singleton

        private static MultipanelDAO Instance;
        private static readonly object instanceLock = new object();

        private MultipanelDAO()
        {
        }

        public static MultipanelDAO getInstance()
        {
            if (Instance == null)
            {
                lock (instanceLock)
                {
                    Instance = new MultipanelDAO();
                }
            }
            return Instance;
        }

        #endregion singleton

        public override List<Multipanel> getAll()
        {
            string request = "SELECT tblpanels_multipanel.ID_panel_displayer, " +
                "(SELECT CONCAT(tblpanels.ID_pan, tblpanels.ID_mac) FROM tblpanels  WHERE tblpanels.ID_panel = ID_panel_displayer) AS PanMacDispalyer, " +
                " tblpanels_multipanel.ID_panel_counter, " +
                "(SELECT CONCAT(tblpanels.ID_pan, tblpanels.ID_mac) FROM tblpanels  WHERE tblpanels.ID_panel = ID_panel_counter) AS PanMacCounter, " +
                "tblpanels_multipanel.counter_mask " +
                "FROM tblpanels_multipanel;";
            MySqlTools sql = MySqlTools.getConnection();
            List<Multipanel> lstMultipanel = new List<Multipanel>();

            DataTable data = sql.executeRequest(request);

            foreach (DataRow row in data.Rows)
            {
                Multipanel multi = Constructeur<Multipanel>.createInstance(row);
                multi.InitObj();
                lstMultipanel.Add(multi);
            }

            return lstMultipanel;
        }

        public override bool Insert(Multipanel obj)
        {
            throw new NotImplementedException();
        }

        public override bool Update(Multipanel obj)
        {
            throw new NotImplementedException();
        }
    }
}