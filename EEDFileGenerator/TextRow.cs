using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EEDFileGenerator
{
    class TextRow : ICollection<Field<string>>
    {
        private List<Field<string>> inner_list;
        private int id;

        public int ID { get { return this.id; } }

        public TextRow(int id)
        {
            this.inner_list = new List<Field<string>>();
            this.id = id;
        }

        public void Add(Field<string> item)
        {
            if (item == null)
                throw new ArgumentNullException("Item cannot be null.");

            this.inner_list.Add(item);

        }

        public Field<string> GetField(int id)
        {
            return inner_list.Find(x => x.ID == id);
        }


        public void Clear()
        {
            this.inner_list.Clear();
        }

        public bool Contains(Field<string> item)
        {
            return (this.inner_list.Where(T => T.Value == item.Value).FirstOrDefault() != null);
        }

        public void CopyTo(Field<string>[] array, int arrayIndex)
        {
            List<Field<string>> temp_list = new List<Field<string>>();
            temp_list.AddRange(this.inner_list);
            temp_list.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return this.inner_list.Count(); }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(Field<string> item)
        {
            if (item != null)
            {
                if (this.inner_list.Remove(item))
                {
                    return true;
                }
            }
            return false;
        }

        public IEnumerator<Field<string>> GetEnumerator()
        {
            foreach (var field in this.inner_list)
            {
                yield return field;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
