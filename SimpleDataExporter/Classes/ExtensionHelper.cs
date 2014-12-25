using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SimpleDataExporter.Classes
{
    internal static class ExtensionHelper
    {
        internal static string ToOPT(this DataTable table)
        {
            var result = new StringBuilder();

            foreach (DataRow row in table.Rows)
            {

                for (int i = 0; i < table.Columns.Count; i++)
                {
                    result.Append(i == table.Columns.Count - 1 ? row[i].ToString() + "\r\n" : row[i].ToString() + ",");
                }
            }
            return result.ToString();
        }

        internal static string ToLFP(this DataTable table)
        {
            var result = new StringBuilder();

            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    if (i != table.Columns.Count - 1)
                    {
                        switch (i)
                        {
                            case 4:
                            case 5:
                            case 6:
                                result.Append(row[i].ToString() + ";");
                                break;

                            default:
                                result.Append(row[i].ToString() + ",");
                                break;
                        }

                    }
                    else
                    {
                        result.Append(row[i].ToString() + "\r\n");
                    }
                }
            }
            return result.ToString();
        }

        internal static string ToPipeCarot(this DataTable table)
        {
            var result = new StringBuilder();


            for (int i = 0; i < table.Columns.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        result.Append(i == table.Columns.Count - 1 ? "|\r\n" : "^" + table.Columns[i].ColumnName + "^");
                        break;
                    default:
                        result.Append(i == table.Columns.Count - 1 ? "|\r\n" : "|^" + table.Columns[i].ColumnName + "^");
                        break;
                }
            }

            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {

                    string strRow = row[i].ToString();
                    strRow = strRow.Replace("\r\n", Convert.ToChar(59).ToString());
                    strRow = strRow.Replace("\n", Convert.ToChar(32).ToString());

                    switch (i)
                    {
                        case 0:
                            result.Append(i == table.Columns.Count - 1 ? "|\r\n" : "^" + strRow + "^");
                            break;
                        default:
                            result.Append(i == table.Columns.Count - 1 ? "|\r\n" : "|^" + strRow + "^");
                            break;
                    }

                }
            }
            return result.ToString();
        }

        internal static string ToDatDelimiter(this DataTable table)
        {
            char fieldChar = Convert.ToChar(20);
            char quoteChar = Convert.ToChar(254);
            char multiEntryChar = Convert.ToChar(59);
            char endOfLineChar = Convert.ToChar(174);
            StringBuilder result = new StringBuilder();
            string stripColumnName = String.Empty;

            for (int i = 0; i < table.Columns.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        result.Append(quoteChar.ToString() + table.Columns[i].ColumnName + quoteChar.ToString());
                        break;
                    default:
                        result.Append(i == table.Columns.Count - 1 ? "\r\n" : fieldChar.ToString() + quoteChar.ToString() + table.Columns[i].ColumnName + quoteChar.ToString());
                        break;
                }
            }

            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    string strRow = row[i].ToString();
                    strRow = strRow.Replace("\r\n", multiEntryChar.ToString());
                    strRow = strRow.Replace("\n", endOfLineChar.ToString());

                    switch (i)
                    {
                        case 0:
                            result.Append(i == table.Columns.Count - 1 ? "\r\n" : quoteChar.ToString() + strRow + quoteChar.ToString());
                            break;
                        default:
                            result.Append(i == table.Columns.Count - 1 ? "\r\n" : fieldChar.ToString() + quoteChar.ToString() + strRow + quoteChar.ToString());
                            break;
                    }
                }
            }
            return result.ToString();
        }

        internal static string ToCSV(this DataTable table)
        {
            var result = new StringBuilder();

            foreach (DataRow row in table.Rows)
            {

                for (int i = 0; i < table.Columns.Count; i++)
                {
                    result.Append(i == table.Columns.Count - 1 ? row[i].ToString() + "\r\n" : row[i].ToString() + ",");
                }
            }
            return result.ToString();
        }

        internal static DataTable RemoveUnusedColumns(this DataTable table)
        {
            for (int col = table.Columns.Count - 1; col >= 0; col--)
            {
                bool removeColumn = true;
                foreach (DataRow row in table.Rows)
                {
                    if (!row.IsNull(col))
                    {
                        removeColumn = false;
                        break;
                    }
                }
                if (removeColumn)
                    table.Columns.RemoveAt(col);
            }

            return table;
        }
    }
}

