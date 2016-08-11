using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EEDFileGenerator
{
    internal sealed class TextRows : ICollection<TextRow>
    {
        private List<TextRow> inner_list;

        public TextRows()
        {
            inner_list = new List<TextRow>();
        }

        public void Add(TextRow item)
        {
            if (item == null)
                throw new ArgumentNullException("Item cannot be null.");

            if (inner_list.Any(i => i.ID == item.ID))
                throw new Exception("Duplicate ID for TextRow.");

            this.inner_list.Add(item);
        }

        public void Clear()
        {
            this.inner_list.Clear();
        }

        public bool Contains(TextRow item)
        {
            return (this.inner_list.Any(i => i.ID == item.ID));
        }

        public void CopyTo(TextRow[] array, int arrayIndex)
        {
            List<TextRow> temp_list = new List<TextRow>();
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

        public bool Remove(TextRow item)
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

        public IEnumerator<TextRow> GetEnumerator()
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
