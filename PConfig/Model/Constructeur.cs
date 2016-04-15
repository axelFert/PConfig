using System;
using System.Data;
using System.Reflection;

namespace PConfig.Model
{
    /// <summary>
    /// Constructeur universel d'objet à partir d'une ligne de donnée En utilisant la Reflexion
    /// vraiment très pratique
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class Constructeur<T>
    {
        /// <summary>
        /// Creation d'un objet de la classe
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static T createInstance(DataRow row)
        {
            Type type = typeof(T);
            ConstructorInfo ctor = type.GetConstructor(new Type[0]);
            T instance = (T)(ctor.Invoke(new object[0]));

            foreach (PropertyInfo prop in type.GetProperties())
            {
                if (row.Table.Columns.Contains(prop.Name))
                    prop.SetValue(instance, Convert.ChangeType(row[prop.Name], prop.PropertyType));
            }
            return instance;
        }

        private Constructeur()
        {
        }
    }
}