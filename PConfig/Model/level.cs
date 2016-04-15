namespace PConfig.Model
{
    public class Level
    {
        public string name { get; set; }
        public int ID_level { get; set; }
        public string fileNamePlan { get; set; }

        /// <summary>
        /// juste un constructeur vide. la classe <see cref="Constructeur{T}"/> se charge
        /// d'instancier la classe
        /// </summary>
        public Level()
        {
        }
    }
}