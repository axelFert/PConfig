using System.Collections.Generic;

namespace PConfig.Conf
{
    public class Configuration
    {
        public string NomSite { get; set; }

        /// <summary>
        /// Tuple avec le nom et l'adresse du niveau
        /// </summary>
        public List<PlanInfo> ListePlan { get; set; }

        public string HostName { get; set; }

        public string Port { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public bool isOnMasterData { get; set; }

        public Configuration()
        {
            ListePlan = new List<PlanInfo>();
        }

        public override string ToString()
        {
            return NomSite;
        }
    }
}