using PConfig.Tools.Mysql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PConfig.Model.DAO
{
    internal class HubDAO : SmgDao<Hub>
    {
        #region singleton

        private HubDAO()
        {
        }

        static HubDAO()
        {
        }

        private static HubDAO _instance = new HubDAO();
        public static HubDAO Instance { get { return _instance; } }

        #endregion singleton

        public override List<Hub> getAll()
        {
            string requete = "select * from tblconfiguration_hubs";
            MySqlTools sql = MySqlTools.getConnection();
            List<Hub> lstHub = new List<Hub>();

            DataTable data = sql.executeRequest(requete);

            foreach (DataRow row in data.Rows)
            {
                Hub hub = Constructeur<Hub>.createInstanceFields(row);
                lstHub.Add(hub);
            }
            return lstHub;
        }

        public override bool Insert(Hub obj)
        {
            throw new NotImplementedException();
        }

        public override bool Update(Hub obj)
        {
            throw new NotImplementedException();
        }
    }
}