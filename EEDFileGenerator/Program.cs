using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TextUtilities;


namespace EEDFileGenerator
{
    class Program
    {
        const int MEMBERIND_ID = 0;
        const int MEMBER_ID = 1;
        const int PACKAGE_ID = 2;
        const int PBP_ID = 3;
        const int HCONTRACT = 4;
        const int EFFE_DATE = 5;
        const int TERM_DATE = 6;
        const string OUTPUT_FILE_NAME = "eed_file.csv";

        static TextRows input_rows = new TextRows();
        static List<string> output_rows = new List<string>();


        static void Main(string[] args)
        {
            string source_file = args[0].ToString();
            if (string.IsNullOrEmpty(source_file.Trim()))
            {
                System.Console.WriteLine("Must specify name of the file to be processed.");
                return;
            }

            System.Console.WriteLine("Processing file '{0}':", source_file);
            System.Console.Write("Loading file...");
            List<string> lines = LoadFile(source_file);
            System.Console.WriteLine(" Loaded.");
            System.Console.Write("Processing...");
            Process(lines);
            System.Console.WriteLine(" Processed.");
            System.Console.Write("Creating new file...");
            CreateFile();
            System.Console.WriteLine("Finished.");
        }

        static List<string> LoadFile(string file_name)
        {
            List<string> lines = new List<string>();

            using (StreamReader sr = File.OpenText( file_name ))
            {
                string line = null;

                while ((line = sr.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            return lines;
        }

        static void Process(List<string> lines)
        {
            int counter = 1;
            string prev_member_id, prev_pbp_id, prev_hcontract, prev_effective_date, prev_term_date, enrollment_effective_date;
            prev_member_id = prev_pbp_id = prev_hcontract = prev_effective_date = prev_term_date = enrollment_effective_date = string.Empty;

            foreach (string line in lines)
            {
                TextRow row = GetRow( line, counter++ );
                string curr_member_id, curr_pbp_id, curr_hcontract, curr_effective_date, curr_term_date;
                curr_member_id = row.GetField(MEMBER_ID).Value; curr_pbp_id = row.GetField(PBP_ID).Value;
                curr_hcontract = row.GetField(HCONTRACT).Value; curr_effective_date = row.GetField(EFFE_DATE).Value;
                curr_term_date = row.GetField(TERM_DATE).Value;
                if (curr_member_id == prev_member_id)
                {
                    if (IsException(curr_hcontract, curr_pbp_id, prev_hcontract, prev_pbp_id)) //added
                    {
                        enrollment_effective_date = prev_effective_date;
                    }
                    else if ((curr_pbp_id == prev_pbp_id) && (curr_hcontract == prev_hcontract))
                    {
                        if (string.IsNullOrEmpty(prev_term_date.Trim()))
                        {
                            enrollment_effective_date = curr_effective_date;
                        }
                        else
                        {
                            DateTime lead_date = Convert.ToDateTime(prev_term_date).AddMonths(1);
                            if (lead_date <= Convert.ToDateTime(curr_effective_date))
                            {
                                enrollment_effective_date = curr_effective_date;
                            }
                            /*double diff_in_days = (Convert.ToDateTime(curr_effective_date) - Convert.ToDateTime(prev_term_date)).TotalDays;
                            if (diff_in_days >= 30)
                            {
                                enrollment_effective_date = curr_effective_date;
                            }*/
                        }
                    }
                    else
                    {
                        enrollment_effective_date = curr_effective_date;
                    }
                }
                else
                {
                    prev_member_id = curr_member_id;
                    enrollment_effective_date = curr_effective_date;
                }
                prev_pbp_id = curr_pbp_id; prev_hcontract = curr_hcontract;
                prev_effective_date = curr_effective_date; prev_term_date = curr_term_date;
                AddRecord(curr_member_id, curr_pbp_id, curr_hcontract, curr_effective_date, curr_term_date, enrollment_effective_date);
            }
        }

        static bool IsException(string curr_hcontract, string curr_pbp_id,
                         string prev_hcontract, string prev_pbp_id)
        {
            bool result = false;
        
            if ( (prev_hcontract == "H5887" && prev_pbp_id == "007" && curr_hcontract == "H4003" && curr_pbp_id == "009") ||
                 (prev_hcontract == "H5887" && prev_pbp_id == "010" && curr_hcontract == "H4003" && curr_pbp_id == "017") 
               )
               result = true;

            return result;
        }

        static void AddRecord(string member_id, string pbp_id, string hcontract, 
            string effective_date, string term_date, string eed)
        {
            //string new_record = string.Format("{0}|{1}|{2}|{3}|{4}|{5}",
               //member_id, pbp_id, hcontract, effective_date, term_date, eed);
            string new_record = string.Format("{0}|{1}|{2}|{3}",
                member_id, effective_date, term_date, eed);
            output_rows.Add(new_record);           
        }

        static TextRow GetRow(string line, int idx)
        {
            char[] delimiters = { '|' };
            string[] fields = TextUtilities.TextUtils.Tokenize(delimiters, line);

            TextRow row = new TextRow(idx);
            int fld_count = fields.Length;

            for(int i = 0; i < fld_count; i++)
            {
                row.Add( new Field<string>( fields[i], i ) );
            }
            return row;
        }

        static void CreateFile()
        {
            using (StreamWriter writer = File.CreateText(OUTPUT_FILE_NAME))
            {
                //writer.WriteLine("MEMBER_ID|PBP|HCONTRACT|EFFECTIVE_DATE|TERM_DATE|EED");
                writer.WriteLine("MEMBER_ID|EFFECTIVE_DATE|TERM_DATE|EED");
                foreach(string row in output_rows)
                {
                    writer.WriteLine(row);
                }
            }
        }
    }
}
