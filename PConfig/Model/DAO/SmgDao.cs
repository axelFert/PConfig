using System.Collections.Generic;

namespace PConfig.Model.DAO
{
    public abstract class SmgDao<T>
    {
        public abstract List<T> getAll();

        public abstract bool Update(T obj);

        public abstract bool Insert(T obj);
    }
}