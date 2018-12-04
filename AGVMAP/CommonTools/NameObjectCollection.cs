using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTools
{
    public class NameObjectCollection<T> : List<NameObject<T>>
    {
        public void Add(string name, T value)
        {
            base.Add(new NameObject<T>(name, value));
        }

        public NameObject<T> FindValue(T value)
        {
            NameObject<T> result;
            foreach (NameObject<T> current in this)
            {
                T @object = current.Object;
                bool flag = @object.Equals(value);
                if (flag)
                {
                    result = current;
                    return result;
                }
            }
            result = null;
            return result;
        }
    }
}
