using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace PConfig.Tools
{
    /// <summary>
    /// les différentes varaibles statiques utile
    /// </summary>
    public static class SmgUtil
    {
        public static int MASQUE_HUB_PAN = 0xf00;
        public static int MASQUE_FREQUENCE = 0x0ff;
        public static int MASQUE_TOTEM_RADIO_MAC = 0xff00;
        public static int MASQUE_NUMERO_PLACE_MAC = 0x00ff;

        public enum TYPE_PLACE
        {
            NORMAL, VISITEUR, DESACTIVE, HANDICAPE, ELECTRIQUE
        }

        private static readonly Dictionary<char, string> hexCharacterToBinary = new Dictionary<char, string> {
            { '0', "0000" },
            { '1', "0001" },
            { '2', "0010" },
            { '3', "0011" },
            { '4', "0100" },
            { '5', "0101" },
            { '6', "0110" },
            { '7', "0111" },
            { '8', "1000" },
            { '9', "1001" },
            { 'a', "1010" },
            { 'b', "1011" },
            { 'c', "1100" },
            { 'd', "1101" },
            { 'e', "1110" },
            { 'f', "1111" }
        };

        public static string HexStringToBinary(string hex)
        {
            StringBuilder result = new StringBuilder();
            foreach (char c in hex)
            {
                // This will crash for non-hex characters. You might want to handle that differently.
                result.Append(hexCharacterToBinary[char.ToLower(c)]);
            }
            return result.ToString();
        }

        public static Bitmap CreationImageJuxtapose(List<string> lstPath)
        {
            List<Bitmap> lstImage = new List<Bitmap>();

            foreach (string path in lstPath)
            {
                lstImage.Add(new Bitmap(path));
            }

            int maxHeight = 0;
            int maxwidth = 0;

            foreach (Bitmap btm in lstImage)
            {
                if (btm.Height > maxHeight)
                {
                    maxHeight = btm.Height;
                }
                maxwidth += btm.Width;
            }
            Bitmap imgResult = new Bitmap(maxwidth, maxHeight);

            int actualwidth = 0;
            foreach (Bitmap img in lstImage)
            {
                using (Graphics gfx = Graphics.FromImage(imgResult))
                {
                    gfx.DrawImage(img, new Point(actualwidth, 0));
                }
                actualwidth += img.Width;
            }

            return imgResult;
        }

        public static String HexConverter(System.Windows.Media.Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        public static String HexConverter(System.Drawing.Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        public static String RGBConverter(System.Drawing.Color c)
        {
            return "RGB(" + c.R.ToString() + "," + c.G.ToString() + "," + c.B.ToString() + ")";
        }

        public static String RGBConverter(System.Windows.Media.Color c)
        {
            return "RGB(" + c.R.ToString() + "," + c.G.ToString() + "," + c.B.ToString() + ")";
        }
    }

    public static class EnumUtil
    {
        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}