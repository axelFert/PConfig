using System.Collections.Generic;

namespace PConfig.Model
{
    public interface ILegendObject
    {
        List<Propriete> GetInfo();

        string getNom();

        string getType();
    }
}