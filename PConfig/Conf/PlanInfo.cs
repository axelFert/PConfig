namespace PConfig.Conf
{
    public class PlanInfo
    {
        public int Zone { get; set; }
        public string Nom { get; set; }
        public string Path { get; set; }

        public PlanInfo(int num, string nom, string path)
        {
            Zone = num;
            Nom = nom;
            Path = path;
        }
    }
}