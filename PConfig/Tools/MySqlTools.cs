using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace PConfig.Tools.Mysql
{
    internal class MySqlTools
    {
        /// <summary>
        /// LOGGER
        /// </summary>
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(MySqlTools));

        public string Hostname { get; set; }
        public string Port { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string DbName { get; set; }

        private string myConnection
        {
            get
            {
                return string.Format(@"SERVER={0};PORT={1};DATABASE={2};UID={3};PASSWORD={4};", Hostname, Port, DbName, Login, Password);
            }
        }

        private static MySqlTools instance = null;
        private static bool isInitialized = false;

        private MySqlTools()
        {
            Hostname = "localhost";
            DbName = "db_smg_masterdata";
            Login = "root";
            Password = "";
            //password = "";
            //myConnection = String.Format(@"SERVER={0};DATABASE={1};UID={2};PASSWORD= ;", Hostname, DbName, Login);
        }

        private MySqlTools(string _host, string _port, string _login, string _mdp, string _dbName)
        {
            Hostname = _host;
            Port = _port;
            Login = _login;
            Password = _mdp;
            DbName = _dbName;
            //myConnection = String.Format(@"SERVER={0};PORT={1};DATABASE={2};UID={3};PASSWORD={4};", Hostname, Port, DbName, Login, Password);
        }

        /// <summary>
        /// Connection a la base
        /// </summary>
        public static MySqlTools getConnection()
        {
            if (!isInitialized)
            {
                return null;
            }
            return instance;
        }

        public DataSet executeRequest(string request, params object[] values)
        {
            using (MySqlConnection connection = new MySqlConnection(myConnection))
            {
                try
                {
                    connection.Open();
                    using (MySqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = request;
                        int countParam = 0;
                        foreach (object param in values)
                        {
                            command.Parameters[countParam].Value = param;
                            countParam++;
                        }
                        MySqlDataReader Reader = command.ExecuteReader();
                        MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                        DataSet DataSet = new DataSet();
                        dataAdapter.Fill(DataSet);
                        return DataSet;
                    }
                }
                catch (Exception exception)
                {
                    log.Error("Erreur lors de l'execution de la requete " + exception);
                }
                return null;
            }
        }

        public static void init(string _host, string _port, string _login, string _mdp, string _dbName)
        {
            isInitialized = true;
            instance = new MySqlTools(_host, _port, _login, _mdp, _dbName);
        }

        public void changeDatabase(string dbName)
        {
            this.DbName = dbName;
        }

        public DataTable executeRequest(string request)
        {
            log.Info("execution de la requete :" + request);
            // Create a list to store the result
            DataTable dt = new DataTable();
            using (MySqlConnection connection = new MySqlConnection(myConnection))
            {
                try
                {
                    MySqlDataAdapter adr = new MySqlDataAdapter(request, connection);
                    adr.SelectCommand.CommandType = CommandType.Text;
                    adr.Fill(dt); //opens and closes the DB connection automatically !!
                }
                catch (Exception exception)
                {
                    log.Error("Erreur : ", exception);
                }
            }
            log.Info("La requete à retournée " + dt.Rows.Count + " resultats");
            return dt;
        }

        public bool executeUpdate(string updateRequest)
        {
            log.Info("execution de la requete d'update :" + updateRequest);

            using (MySqlConnection connection = new MySqlConnection(myConnection))
            {
                try
                {
                    using (MySqlDataAdapter adr = new MySqlDataAdapter(updateRequest, connection))
                    {
                        DataSet ds = new DataSet();
                        adr.Fill(ds);
                    }
                }
                catch (Exception exception)
                {
                    log.Error("Erreur : ", exception);
                }
            }
            return true;
        }
    }
}