using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEDFileGenerator
{
    internal sealed class Field<T>
    {
        private T value;
        private int id;

        public T Value { get { return this.value; } }
        public int ID { get { return this.id; } }

        private Field() { }
        public Field(T value)
        {
            this.value = value;
        }

        public Field(T value, int id)
        {
            this.value = value;
            this.id = id;
        }

        public T GetValue()
        {
            return this.value;
        }
    }
}
