using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Metaproject.Database
{
    public interface IDatabaseHelper
    {

        SqlScipt GetCreateTableScript(string connectionString, string tableName);
        SqlScipt GetCreateTableScripts(DatabaseInfo databaseInfo);

        void CreateDatabase(string serverName, string databaseNewName);
        void ExecuteScript(string server, string database, SqlScipt script);


    }
}
