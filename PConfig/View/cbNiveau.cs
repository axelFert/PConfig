using System;
using System.Windows;
using System.Windows.Controls;

namespace PConfig.View
{
    internal class cbNiveau : CheckBox
    {
        public string Path { get; private set; }
        public string Nom { get; private set; }
        public int IdZone { get; private set; }

        public cbNiveau(int numero, string nom, string path)
        {
            IdZone = numero; Nom = nom; Path = path;
            Name = Nom;
            Content = Nom;
            Margin = new Thickness(0, 5, 5, 0);
            IsChecked = true;
        }
    }
}