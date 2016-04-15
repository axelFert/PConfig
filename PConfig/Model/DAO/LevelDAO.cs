using PConfig.Tools.Mysql;
using System;
using System.Collections.Generic;
using System.Data;

namespace PConfig.Model.DAO
{
    internal class LevelDAO : SmgDao<Level>
    {
        /// <summary>
        /// LOGGER
        /// </summary>
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(LevelDAO));

        #region singleton

        private static LevelDAO Instance;
        private static readonly object instanceLock = new object();

        private LevelDAO()
        {
        }

        public static LevelDAO getInstance()
        {
            if (Instance == null)
            {
                lock (instanceLock)
                {
                    Instance = new LevelDAO();
                }
            }
            return Instance;
        }

        #endregion singleton

        public override List<Level> getAll()
        {
            string requete = "select * from tblLevels";
            MySqlTools sql = MySqlTools.getConnection();
            List<Level> lstLevel = new List<Level>();

            DataTable data = sql.executeRequest(requete);

            foreach (DataRow row in data.Rows)
            {
                Level level = Constructeur<Level>.createInstance(row);
                lstLevel.Add(level);
            }
            return lstLevel;
        }

        public override bool Insert(Level obj)
        {
            throw new NotImplementedException();
        }

        public override bool Update(Level obj)
        {
            throw new NotImplementedException();
        }
    }
}