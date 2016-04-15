using PConfig.Conf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace PConfig.Tools
{
    public class XmlParser
    {
        /// <summary>
        /// LOGGER
        /// </summary>
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(XmlParser));

        public static void readXmldoc(string fileName)
        {
            string curpath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string file = curpath + "\\" + fileName;
            if (File.Exists(file))
            {
                XElement xelement = XElement.Load(file);
                IEnumerable<XElement> sites = xelement.Elements();
                // Read the entire XML
                foreach (XElement site in sites)
                {
                    Console.WriteLine(site);
                }
            }
        }

        public static List<string> getParcs(string fileName)
        {
            List<string> lstNomSites = new List<string>();
            string curpath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string file = curpath + "\\" + fileName;
            if (File.Exists(file))
            {
                XElement xelement = XElement.Load(file);
                IEnumerable<XElement> sites = xelement.Elements();
                // Read the entire XML
                foreach (XElement site in sites)
                {
                    lstNomSites.Add(site.Attribute("name").Value);
                }
            }
            return lstNomSites;
        }

        /// <summary>
        /// Methode qui va lire le fichier de conf et en extraire la conf de tout les site
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static List<Configuration> GetConfig(string fileName)
        {
            List<Configuration> retour = new List<Configuration>();
            string curpath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string file = curpath + "\\" + fileName;
            if (File.Exists(file))
            {
                try
                {
                    XmlDocument ConfFile = new XmlDocument();
                    ConfFile.Load(file);
                    foreach (XmlNode site in ConfFile.DocumentElement.ChildNodes)
                    {
                        Configuration conf = new Configuration();
                        conf.NomSite = site.Attributes["name"].Value;
                        XmlNode dbInfo = site.SelectSingleNode("dbinfo");
                        conf.HostName = dbInfo.SelectSingleNode("hostname").InnerText;
                        conf.Port = dbInfo.SelectSingleNode("port").InnerText;
                        conf.Login = dbInfo.SelectSingleNode("login").InnerText;
                        conf.Password = dbInfo.SelectSingleNode("password").InnerText;
                        XmlNode planElement = site.SelectSingleNode("plan");
                        foreach (XmlNode niveau in planElement.ChildNodes)
                        {
                            PlanInfo vPlan = new PlanInfo(Int32.Parse(niveau.Attributes["id_zone"].Value), niveau.Attributes["name"].Value, niveau.InnerText);
                            conf.ListePlan.Add(vPlan);
                        }
                        retour.Add(conf);
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Erreur lors de la lecture du fichier de conf : ", ex);
                }
            }
            return retour;
        }
    }
}