using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PConfig.Model
{
    internal class Hub : ILegendObject
    {
        private int hubNumber;

        public int Numero
        {
            get { return hubNumber; }
            set { hubNumber = value; }
        }

        private string linkType;

        public string Type
        {
            get { return linkType; }
            set { linkType = value; }
        }

        private int hubFrequency;

        public int Frequence
        {
            get { return hubFrequency; }
            set { hubFrequency = value; }
        }

        private string tcpPort;

        public string PortTcp
        {
            get { return tcpPort; }
            set { tcpPort = value; }
        }

        private string portName;

        public string NomPort
        {
            get { return portName; }
            set { portName = value; }
        }

        private string tcpHost;

        public string Host
        {
            get { return tcpHost; }
            set { tcpHost = value; }
        }

        private string nickname;

        public string Nom
        {
            get { return nickname; }
            set { nickname = value; }
        }

        public List<Propriete> GetInfo()
        {
            List<Propriete> lst = new List<Propriete>();
            lst.Add(new Propriete("Numero", Numero.ToString()));
            lst.Add(new Propriete("Frequence", Frequence.ToString()));
            lst.Add(new Propriete("Type ", Type));
            if (Type.Equals("tcp"))
            {
                lst.Add(new Propriete("Host", Host));
                lst.Add(new Propriete("Port tcp", PortTcp));
            }
            else
            {
                lst.Add(new Propriete("Nom port", NomPort));
            }
            return lst;
        }

        public string getNom()
        {
            return Nom;
        }

        public string getType()
        {
            return "Hub";
        }
    }
}