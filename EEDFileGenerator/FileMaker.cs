using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace EEDFileGenerator
{
    internal sealed class FileMaker : ITextRows
    {
        private string file_name;
        private char? delimiter;

        private FileMaker() { }
        public FileMaker(string file_name, char? delimiter)
        {
            if (String.IsNullOrEmpty(file_name))
                throw new Exception("Null or Empty file name.");

            if (!delimiter.HasValue)
                throw new Exception("Null delimiter.");

            this.file_name = file_name.Trim();
            this.delimiter = delimiter;

        }

        public bool CreateFile(TextRows rows)
        {
            using (StreamWriter writer = File.CreateText(this.file_name))
            {
                StringBuilder str = new StringBuilder();
                foreach (TextRow row in rows)
                {
                    int field_count = row.Count();
                    int counter = 1;
                    foreach (Field<string> field in row)
                    {
                        str.Append(field.GetValue());
                        if (counter < field_count)
                            str.Append(this.delimiter.ToString());
                        counter++;
                    }
                    writer.WriteLine(str.ToString());
                    str.Clear();
                }
            }
            return true;
        }
    }
}
