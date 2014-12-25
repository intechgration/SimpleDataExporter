using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using ADODB;
using System.Configuration;

namespace SimpleDataExporter.Classes
{
    internal class DatabaseOperations
    {
        internal static DataTable GetTableSchema(string connectionString)
        {
            DataTable schemaTable = new DataTable();

            using (OleDbConnection oleConn = new OleDbConnection(connectionString))
            {
                oleConn.Open();
                schemaTable = oleConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            }

            return schemaTable;
        }


        internal static void GetCoreDB(dynamic swObj)
        {
            Recordset coreDBRecordSet = swObj.DB.Clone();
        }

        internal static DataTable GetSpecificTable(string query, string connectionString)
        {

            DataTable specificTable = new DataTable();

            using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, connectionString))
            {
                adapter.Fill(specificTable);
            }

            return specificTable;
        }

        internal static string GetDBConnectionString(string dbPath)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BaseConnectionString"].ConnectionString + dbPath;
            return connectionString;
        }

        internal static string GetBaseConnectionString()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BaseConnectionString"].ConnectionString;
            return connectionString;
        }

    }
}
