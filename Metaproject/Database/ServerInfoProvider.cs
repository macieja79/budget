using System;
using System.Data;
using System.Data.SqlClient;

namespace Metaproject.Database
{
    public class ServerInfoProvider
    {

        public static ServerInfo GetServerInfo(string server, string user, string pass)
        {

            ServerInfo serverInfo = new ServerInfo() { Name = server };

            string connectionString = @"Data Source=" + server + ";uid=" + user + ";pwd=" + pass;

            FillDatabases(serverInfo, connectionString);

            foreach (DatabaseInfo databaseInfo in serverInfo.Databases)
                FillTables(databaseInfo, connectionString);

            return serverInfo;
        }

        static void FillDatabases(ServerInfo serverInfo, string connectionString)
        {

            string selectCommandText = "select name from sys.databases order by name";
            SqlConnection selectConnection = new SqlConnection(connectionString);

            try
            {
                selectConnection.Open();
                DataTable dataTable = new DataTable();
                new SqlDataAdapter(selectCommandText, selectConnection).Fill(dataTable);
                selectConnection.Close();

                foreach (DataRow row in dataTable.Rows)
                {
                    string databaseName = (string)row["name"];
                    DatabaseInfo databaseInfo = new DatabaseInfo() { Name = databaseName };
                    serverInfo.Databases.Add(databaseInfo);
                }
            }
            catch (Exception ex)
            { }
        }

        static void FillTables(DatabaseInfo dabaseInfo, string connectionString)
        {
            string selectCommandText = "USE " + dabaseInfo.Name + " SELECT * FROM sys.Tables ";
            SqlConnection selectConnection = new SqlConnection(connectionString);

            try
            {
                selectConnection.Open();
                DataTable dataTable = new DataTable();
                new SqlDataAdapter(selectCommandText, selectConnection).Fill(dataTable);
                selectConnection.Close();

                foreach (DataRow row in dataTable.Rows)
                {
                    string tableName = (string)row["name"];
                    TableInfo tableInfo = new TableInfo() { Name = tableName };
                    dabaseInfo.Tables.Add(tableInfo);
                }
            }
            catch (Exception ex)
            { }
        }

    }
}