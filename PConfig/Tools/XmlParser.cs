using PConfig.Conf;
using PConfig.View.Utils;
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
        public static Configuration GetConfig(string fileName)
        {
            Configuration retour = new Configuration();
            string curpath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string file = curpath + "\\" + fileName;
            if (File.Exists(file))
            {
                try
                {
                    XmlDocument ConfFile = new XmlDocument();
                    ConfFile.Load(file);
                    foreach (XmlNode node in ConfFile.DocumentElement.ChildNodes)
                    {
                        if (node.Name.Contains("application"))
                        {
                            retour = chargerConfiguration(retour, node);
                        }
                        else
                        {
                            retour = chargerSite(retour, node);
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Erreur lors de la lecture du fichier de conf : ", ex);
                }
            }
            return retour;
        }

        private static Configuration chargerConfiguration(Configuration conf, XmlNode parent)
        {
            foreach (XmlNode child in parent)
            {
                if (child.Name.Equals("mat"))
                {
                    conf.TailleMat = Int32.Parse(child.SelectSingleNode("taille").InnerText);
                    conf.CouleurMat = child.SelectSingleNode("couleur").InnerText;
                }
                else if (child.Name.Equals("totem"))
                {
                    conf.TailleTotem = Int32.Parse(child.SelectSingleNode("taille").InnerText);
                    conf.CouleurTotem = child.SelectSingleNode("couleur").InnerText;
                }
                else if (child.Name.Equals("place"))
                {
                    conf.CouleurPlace = child.SelectSingleNode("couleur").InnerText;
                }
            }

            return conf;
        }

        private static Configuration chargerSite(Configuration conf, XmlNode node)
        {
            List<ConfigurationSite> lst = new List<ConfigurationSite>();
            foreach (XmlNode site in node)
            {
                ConfigurationSite config = new ConfigurationSite();
                config.NomSite = site.Attributes["name"].Value;
                XmlNode dbInfo = site.SelectSingleNode("dbinfo");
                config.HostName = dbInfo.SelectSingleNode("hostname").InnerText;
                config.Port = dbInfo.SelectSingleNode("port").InnerText;
                config.Login = dbInfo.SelectSingleNode("login").InnerText;
                config.Password = dbInfo.SelectSingleNode("password").InnerText;
                XmlNode planElement = site.SelectSingleNode("plan");
                foreach (XmlNode niveau in planElement.ChildNodes)
                {
                    PlanInfo vPlan = new PlanInfo(Int32.Parse(niveau.Attributes["id_zone"].Value), niveau.Attributes["name"].Value, niveau.InnerText);
                    config.ListePlan.Add(vPlan);
                }
                lst.Add(config);
            }
            conf.LstSite = lst;
            return conf;
        }

        public static void sauvegarderConf(string fileName)
        {
            string curpath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string file = curpath + "\\" + fileName;
            if (File.Exists(file))
            {
                try
                {
                    XmlDocument ConfFile = new XmlDocument();
                    ConfFile.Load(file);
                    foreach (XmlNode node in ConfFile.DocumentElement.ChildNodes)
                    {
                        if (node.Name.Contains("application"))
                        {
                            foreach (XmlNode child in node)
                            {
                                if (child.Name.Equals("mat"))
                                {
                                    child.SelectSingleNode("taille").InnerText = SmgUtilsIHM.COTE_MAT + "";
                                    child.SelectSingleNode("couleur").InnerText =
                                        SmgUtil.HexConverter(SmgUtilsIHM.getColorEtat(ETAT_OBJET_PLAN.NONE_MAT).CouleurBordure);
                                }
                                else if (child.Name.Equals("totem"))
                                {
                                    child.SelectSingleNode("taille").InnerText = SmgUtilsIHM.COTE_MAT + "";
                                    child.SelectSingleNode("couleur").InnerText =
                                        SmgUtil.HexConverter(SmgUtilsIHM.getColorEtat(ETAT_OBJET_PLAN.NONE_TOTEM).CouleurBordure);
                                }
                                else if (child.Name.Equals("place"))
                                {
                                    child.SelectSingleNode("couleur").InnerText =
                                        SmgUtil.HexConverter(SmgUtilsIHM.getColorEtat(ETAT_OBJET_PLAN.NONE_PLACE).CouleurBordure);
                                }
                            }
                        }
                    }
                    ConfFile.Save(file);
                }
                catch
                {
                }
            }
        }
    }
}