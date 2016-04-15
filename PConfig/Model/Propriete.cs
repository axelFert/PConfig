namespace PConfig.Model
{
    public class Propriete
    {
        public string Nom { get; set; }

        public string Valeur { get; set; }

        public Propriete(string nom, string valeur)
        {
            this.Nom = nom;
            this.Valeur = valeur;
        }
    }
}