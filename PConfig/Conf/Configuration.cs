using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PConfig.Conf
{
    public class Configuration
    {
        public List<ConfigurationSite> LstSite { get; set; }
        public string CouleurMat { get; set; }
        public int TailleMat { get; set; }
        public string CouleurTotem { get; set; }
        public int TailleTotem { get; set; }
        public string CouleurPlace { get; set; }
    }
}