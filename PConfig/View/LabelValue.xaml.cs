using System;
using System.Windows.Controls;

namespace PConfig.View
{
    /// <summary>
    /// Logique d'interaction pour LabelValue.xaml
    /// </summary>
    public partial class LabelValue : UserControl
    {
        public LabelValue(string prop, string value, bool isActive)
        {
            InitializeComponent();
            NomProp.Content = prop + ":";
            ValueProp.Text = value;
            ValueProp.IsEnabled = isActive;
        }

        public string getValueProp
        {
            get
            {
                return ValueProp.Text;
            }
        }
    }
}