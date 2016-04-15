using PConfig.Tools;
using System;

namespace PConfig.Model
{
    public class Multipanel
    {
        public int ID_panel_displayer { get; set; }
        public int ID_panel_counter { get; set; }
        public int counter_id { get; set; }
        public string counter_mask { get; set; }
        public string NewMask { get; set; }

        public int PanMacDispalyer { get; set; }
        public int PanMacCounter { get; set; }

        /// <summary>
        /// constructeur vide
        /// </summary>
        public Multipanel()
        {
        }

        public void InitObj()
        {
            for (int i = 0; i < counter_mask.Length; i += 2)
            {
                string sub = string.Concat(counter_mask[i], counter_mask[i + 1]);
                char[] charArray = SmgUtil.HexStringToBinary(sub).ToCharArray();
                Array.Reverse(charArray);
                NewMask += new string(charArray);
            }
        }
    }
}