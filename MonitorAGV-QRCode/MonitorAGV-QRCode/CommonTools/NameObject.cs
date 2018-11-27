using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTools
{
    public class NameObject<T>
    {
        public string m_name = string.Empty;

        public T m_object;

        public string Name
        {
            get
            {
                return this.m_name;
            }
            set
            {
                this.m_name = value;
            }
        }

        public T Object
        {
            get
            {
                return this.m_object;
            }
            set
            {
                this.m_object = value;
            }
        }

        public NameObject(string name, T obj)
        {
            this.Name = name;
            this.Object = obj;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
