using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Metaproject.Files
{
    

    /// <summary>
    /// Class to store one CSV row
    /// </summary>
    public class CsvRow : List<string>
    {
        public string LineText { get; set; }

        public override string ToString()
        {
            return LineText;
        }
    }


    public class CsvFile
    {

        CsvRow _header;

        public CsvFile(List<CsvRow> rows, bool hasHeader)
        {
            if (hasHeader)
            {
                _header = rows.First();
                Rows = rows.Skip(1).ToList();
            }
            else
            {
                Rows = rows.ToList();
            }
        }


        public int NumberOfRows
        {
            get
            {
                return Rows.Count;
            }
        }


        public List<CsvRow> Rows { get; private set; }

        public string GetValue(string headerName, int rowNumber)
        {
            int index = _header.IndexOf(headerName);
            return Rows[rowNumber][index];
        }

        public bool HasHeader
        {
            get
            {
                return (null != _header);
            }
        }

    }





    public class CsvFileWriter : StreamWriter
    {
        public CsvFileWriter(Stream stream)
            : base(stream)
        {
        }

        public CsvFileWriter(string filename)
            : base(filename)
        {
        }

        /// <summary>
        /// Writes a single row to a CSV file.
        /// </summary>
        /// <param name="row">The row to be written</param>
        public void WriteRow(CsvRow row)
        {
            StringBuilder builder = new StringBuilder();
            bool firstColumn = true;
            foreach (string value in row)
            {
                // Add separator if this isn't the first value
                if (!firstColumn)
                    builder.Append(',');
                // Implement special handling for values that contain comma or quote
                // Enclose in quotes and double up any double quotes
                if (value.IndexOfAny(new char[] { '"', ',' }) != -1)
                    builder.AppendFormat("\"{0}\"", value.Replace("\"", "\"\""));
                else
                    builder.Append(value);
                firstColumn = false;
            }
            row.LineText = builder.ToString();
            WriteLine(row.LineText);
        }
    }

    public class CsvFileReader : StreamReader
    {
        public CsvFileReader(Stream stream)
            : base(stream)
        {
        }

        public CsvFileReader(string filename)
            : base(filename, Encoding.UTF8)
        {
        }


        public static CsvFile GetFile(string fileName, char SEPARATOR, bool hasHeader)
        {

            CsvFileReader reader = new CsvFileReader(fileName);

            List<CsvRow> rows = new List<CsvRow>();

            CsvRow iRow = new CsvRow();
            while (reader.ReadRow(iRow, SEPARATOR))
            {
                rows.Add(iRow);
                iRow = new CsvRow();
            }


            CsvFile file = new CsvFile(rows, hasHeader);
            return file;

        }



        public bool ReadRow(CsvRow row, char SEPARATOR)
        {
            row.LineText = ReadLine();
            if (String.IsNullOrEmpty(row.LineText))
                return false;

            int pos = 0;
            int rows = 0;

            while (pos < row.LineText.Length)
            {
                string value;

                // Special handling for quoted field
                if (row.LineText[pos] == '"')
                {
                    // Skip initial quote
                    pos++;

                    // Parse quoted value
                    int start = pos;
                    while (pos < row.LineText.Length)
                    {
                        // Test for quote character
                        if (row.LineText[pos] == '"')
                        {
                            // Found one
                            pos++;

                            // If two quotes together, keep one
                            // Otherwise, indicates end of value
                            if (pos >= row.LineText.Length || row.LineText[pos] != '"')
                            {
                                pos--;
                                break;
                            }
                        }
                        pos++;
                    }
                    value = row.LineText.Substring(start, pos - start);
                    value = value.Replace("\"\"", "\"");
                }
                else
                {
                    // Parse unquoted value
                    int start = pos;
                    while (pos < row.LineText.Length && row.LineText[pos] != SEPARATOR)
                        pos++;
                    value = row.LineText.Substring(start, pos - start);
                }

                // Add field to list
                if (rows < row.Count)
                    row[rows] = value;
                else
                    row.Add(value);
                rows++;

                // Eat up to and including next comma
                while (pos < row.LineText.Length && row.LineText[pos] != SEPARATOR)
                    pos++;
                if (pos < row.LineText.Length)
                    pos++;
            }
            // Delete any unused items
            while (row.Count > rows)
                row.RemoveAt(rows);

            // Return true if any columns read
            return (row.Count > 0);
        }
    }

}


